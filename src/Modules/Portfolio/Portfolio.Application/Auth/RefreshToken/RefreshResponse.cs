namespace Portfolio.Application.Auth.RefreshToken;

public sealed record RefreshResponse(string AccessToken, int ExpiresInSeconds, string RefreshToken, DateTime RefreshExpiresAtUtc);