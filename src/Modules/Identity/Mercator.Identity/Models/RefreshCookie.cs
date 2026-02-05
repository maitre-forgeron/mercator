namespace Mercator.Identity.Models;

public static class RefreshCookie
{
    public const string Name = "mercator_refresh";

    public static CookieOptions Build()
        => new()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = "/auth/refresh",
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        };
}
