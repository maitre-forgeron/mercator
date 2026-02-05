namespace Mercator.Identity.Core.Infrastructure.Models;

public sealed class JwtOptions
{
    public string Issuer { get; init; } = default!;
    public string Audience { get; init; } = default!;
    public string SigningKey { get; init; } = default!;
    public int AccessTokenMinutes { get; init; }
    public int RefreshTokenDays { get; init; }
}
