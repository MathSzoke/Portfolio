using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Auth.LoginLinkedIn;

public sealed record LoginLinkedInCommand(string? Code, string? AccessToken, string? RedirectUri) : ICommand<AuthResponse>;