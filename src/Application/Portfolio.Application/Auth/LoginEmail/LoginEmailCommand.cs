using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Auth.LoginEmail;

public sealed record LoginEmailCommand(string Email, string Password) : ICommand<AuthResponse>;