using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Auth.LoginGoogle;

public sealed record LoginGoogleCommand(string? AccessToken, string? IdToken) : ICommand<AuthResponse>;