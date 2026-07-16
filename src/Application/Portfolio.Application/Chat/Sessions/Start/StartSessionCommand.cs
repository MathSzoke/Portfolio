using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Chat.Sessions.Start;

public sealed record StartSessionCommand(
    Guid RecipientId
) : ICommand<StartSessionResponse>;

public sealed record StartSessionResponse(
    Guid SessionId,
    Guid? SenderId,
    Guid RecipientId,
    string Status,
    DateTime CreatedAt
);
