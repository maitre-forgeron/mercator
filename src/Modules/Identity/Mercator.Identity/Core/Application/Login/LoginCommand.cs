using Mercator.BuildingBlocks.Application.Abstractions.Commands;
using Mercator.Identity.Core.Application.Contracts;
using Mercator.Identity.Core.Domain;
using Mercator.Identity.Core.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mercator.Identity.Core.Application.Login;

public sealed record LoginCommand(LoginRequest Request) : ICommand<AuthResponse>;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, AuthResponse>
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly ITokenService _tokens;
    private readonly IdentityDbContext _db;

    public LoginCommandHandler(UserManager<ApplicationUser> users, ITokenService tokens, IdentityDbContext db)
    {
        _users = users;
        _tokens = tokens;
        _db = db;
    }

    public async Task<AuthResponse> Handle(LoginCommand cmd, CancellationToken ct)
    {
        var (email, password) = cmd.Request;

        var user = await _users.Users.FirstOrDefaultAsync(x => x.Email == email, ct);
        if (user is null)
            throw new UnauthorizedAccessException("Invalid credentials.");

        if (!await _users.CheckPasswordAsync(user, password))
            throw new UnauthorizedAccessException("Invalid credentials.");

        var access = _tokens.CreateAccessToken(user);

        var refreshTokenResult = _tokens.CreateRefreshToken();
        _db.RefreshTokens.Add(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = refreshTokenResult.TokenHash,
            ExpiresAt = refreshTokenResult.ExpiresAt,
            CreatedAt = DateTimeOffset.UtcNow
        });

        await _db.SaveChangesAsync(ct);

        return new AuthResponse(access, refreshTokenResult.PlainToken);
    }
}
