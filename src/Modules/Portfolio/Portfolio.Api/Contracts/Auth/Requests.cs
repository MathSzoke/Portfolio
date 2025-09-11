namespace Portfolio.Api.Contracts.Auth;

public sealed class RegisterEmailRequest
{
    public string FullName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}

public sealed class LoginEmailRequest
{
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
}

public sealed class GoogleLoginRequest
{
    public string? IdToken { get; init; } = null!;
    public string? AccessToken { get; init; } = null!;
}

public sealed class LinkedInLoginRequest
{
    public string? Code { get; init; }
    public string? AccessToken { get; init; }
    public string? RedirectUri { get; init; }
}