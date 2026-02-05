using Mercator.Identity.Core.Application;
using Mercator.Identity.Core.Domain;
using Mercator.Identity.Core.Infrastructure.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Mercator.Identity.Core.Infrastructure.Services;

public sealed class TokenService : ITokenService
{
    private readonly JwtOptions _opt;

    public TokenService(IOptions<JwtOptions> opt) => _opt = opt.Value;

    public string CreateAccessToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Iss, _opt.Issuer),
            new(JwtRegisteredClaimNames.Aud, _opt.Audience),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
        };

        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            claims.Add(new(ClaimTypes.MobilePhone, user.PhoneNumber));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var now = DateTimeOffset.UtcNow;

        var token = new JwtSecurityToken(
            issuer: _opt.Issuer,
            audience: _opt.Audience,
            claims: claims,
            notBefore: now.UtcDateTime,
            expires: now.AddMinutes(_opt.AccessTokenMinutes).UtcDateTime,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshTokenResult CreateRefreshToken()
    {
        // 64 bytes ~ very strong
        var bytes = RandomNumberGenerator.GetBytes(64);
        var plain = Base64UrlEncode(bytes);
        var hash = HashRefreshToken(plain);
        var expires = DateTimeOffset.UtcNow.AddDays(_opt.RefreshTokenDays);
        return new RefreshTokenResult(plain, hash, expires);
    }

    public string HashRefreshToken(string plainToken)
    {
        var bytes = Encoding.UTF8.GetBytes(plainToken);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash); // 64 hex chars
    }

    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
    }
}
