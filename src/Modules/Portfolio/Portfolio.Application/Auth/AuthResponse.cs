using Portfolio.Domain.Users.Enums;

namespace Portfolio.Application.Auth;

public sealed record AuthResponse(
    string AccessToken,
    DateTime ExpiresAtUtc,
    Guid UserId,
    string Email,
    string FullName,
    string? ImageUrl,
    IReadOnlyList<string> Roles,
    UserStatus Status,
    string RefreshToken,
    DateTime RefreshExpiresAtUtc,
    int ExpiresInMinutes);