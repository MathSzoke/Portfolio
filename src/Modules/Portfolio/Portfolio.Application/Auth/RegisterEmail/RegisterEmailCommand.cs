using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Auth;

namespace Portfolio.Application.Auth.RegisterEmail;

public sealed record RegisterEmailCommand(string FullName, string Email, string Password) : ICommand<AuthResponse>;