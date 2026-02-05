namespace Mercator.Identity.Core.Application.Contracts;

public sealed record RegisterRequest(
    string FirstName,
    string LastName,
    string PersonalNumber,
    string MobileNumber,
    string Email,
    string Password);

public sealed record LoginRequest(string Email, string Password);

public sealed record AuthResponse(string AccessToken, string RefreshToken);
