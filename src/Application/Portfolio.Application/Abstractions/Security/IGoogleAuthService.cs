namespace Portfolio.Application.Abstractions.Security;

public sealed record GoogleUserInfo(string Subject, string Email, string Name, string? PictureUrl);

public interface IGoogleAuthService
{
    Task<GoogleUserInfo> ValidateIdToken(string idToken, string expectedClientId, CancellationToken ct);
}
