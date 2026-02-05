using Mercator.BuildingBlocks.Application.Abstractions.Commands;
using Mercator.Identity.Core.Application.Contracts;
using Mercator.Identity.Core.Domain;
using Mercator.Identity.Core.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace Mercator.Identity.Core.Application.Register;

public sealed record RegisterCommand(RegisterRequest Request) : ICommand<AuthResponse>;

public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, AuthResponse>
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly ITokenService _tokens;
    private readonly IdentityDbContext _db;

    public RegisterCommandHandler(UserManager<ApplicationUser> users, ITokenService tokens, IdentityDbContext db)
    {
        _users = users;
        _tokens = tokens;
        _db = db;
    }

    public async Task<AuthResponse> Handle(RegisterCommand cmd, CancellationToken ct)
    {
        var r = cmd.Request;

        var user = new ApplicationUser
        {
            UserName = r.PersonalNumber,
            PersonalNumber = r.PersonalNumber,
            FirstName = r.FirstName,
            LastName = r.LastName,
            PhoneNumber = r.MobileNumber,
            Email = r.Email
        };

        var result = await _users.CreateAsync(user, r.Password);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

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

        return new AuthResponse(access, refreshTokenResult.PlainToken);
    }
}
