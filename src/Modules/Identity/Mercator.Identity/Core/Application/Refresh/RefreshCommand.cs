using Mercator.BuildingBlocks.Application.Abstractions.Commands;
using Mercator.Identity.Core.Application.Contracts;
using Mercator.Identity.Core.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Mercator.Identity.Core.Application.Refresh;

public sealed record RefreshCommand(string RefreshPlainToken) : ICommand<AuthResponse>;

public sealed class RefreshCommandHandler : ICommandHandler<RefreshCommand, AuthResponse>
{
    private readonly IdentityDbContext _db;
    private readonly ITokenService _tokens;

    public RefreshCommandHandler(IdentityDbContext db, ITokenService tokens)
    {
        _db = db;
        _tokens = tokens;
    }

    public async Task<AuthResponse> Handle(RefreshCommand cmd, CancellationToken ct)
    {
        var oldHash = _tokens.HashRefreshToken(cmd.RefreshPlainToken);

        var old = await _db.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.TokenHash == oldHash, ct);

        if (old is null)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        // Reuse detection: if revoked but still used => suspicious. Revoke all tokens for that user.
        if (old.RevokedAt is not null)
        {
            var tokens = await _db.RefreshTokens.Where(t => t.UserId == old.UserId && t.RevokedAt == null).ToListAsync(ct);
            foreach (var t in tokens) t.RevokedAt = DateTimeOffset.UtcNow;
            await _db.SaveChangesAsync(ct);
            throw new UnauthorizedAccessException("Refresh token reuse detected.");
        }

        if (old.ExpiresAt <= DateTimeOffset.UtcNow)
            throw new UnauthorizedAccessException("Refresh token expired.");

        var access = _tokens.CreateAccessToken(old.User);

        var refreshTokenResult = _tokens.CreateRefreshToken();

        old.RevokedAt = DateTimeOffset.UtcNow;
        old.ReplacedByTokenHash = refreshTokenResult.TokenHash;

        _db.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = old.UserId,
            TokenHash = refreshTokenResult.TokenHash,
            ExpiresAt = refreshTokenResult.ExpiresAt,
            CreatedAt = DateTimeOffset.UtcNow
        });

        await _db.SaveChangesAsync(ct);

        return new AuthResponse(access, refreshTokenResult.PlainToken);
    }
}
