using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Auth.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<RefreshResponse>;