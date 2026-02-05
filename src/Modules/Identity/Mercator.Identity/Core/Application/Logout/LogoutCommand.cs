using Mercator.BuildingBlocks.Application.Abstractions.Commands;
using Microsoft.EntityFrameworkCore;

namespace Mercator.Identity.Core.Application.Logout;

public sealed record LogoutCommand(string RefreshPlainToken) : ICommand<bool>;

public sealed class LogoutCommandHandler : ICommandHandler<LogoutCommand, bool>
{
    private readonly IdentityDbContext _db;
    private readonly ITokenService _tokens;

    public LogoutCommandHandler(IdentityDbContext db, ITokenService tokens)
    {
        _db = db;
        _tokens = tokens;
    }

    public async Task<bool> Handle(LogoutCommand cmd, CancellationToken ct)
    {
        var hash = _tokens.HashRefreshToken(cmd.RefreshPlainToken);

        var token = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == hash, ct);
        if (token is null)
        {
            return false;
        }

        token.RevokedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(ct);

        return true;
    }
}