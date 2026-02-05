using Mercator.Identity.Core.Domain;

namespace Mercator.Identity.Core.Application;

public interface ITokenService
{
    string CreateAccessToken(ApplicationUser user);
    RefreshTokenResult CreateRefreshToken();
    string HashRefreshToken(string plainToken);
}

public sealed record RefreshTokenResult(string PlainToken, string TokenHash, DateTimeOffset ExpiresAt);
