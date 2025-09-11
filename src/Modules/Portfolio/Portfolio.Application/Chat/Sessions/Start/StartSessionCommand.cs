using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Chat.Sessions.Start;

public sealed record StartSessionCommand(string Name, string Email) : ICommand<ChatSessionResponse>;
public sealed record ChatSessionResponse(Guid Id, string Name, string Email, string Status, DateTime CreatedAt);