namespace Portfolio.Application.Abstractions.Security;

public sealed record LinkedInUserInfo(string Subject, string Email, string Name, string? PictureUrl);

public interface ILinkedInAuthService
{
    Task<string> ExchangeCodeForAccessToken(string code, string clientId, string clientSecret, string redirectUri, CancellationToken ct);
    Task<LinkedInUserInfo> GetUserInfo(string accessToken, CancellationToken ct);
}
