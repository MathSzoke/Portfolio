using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Chat.Messages.Post;

public sealed record PostMessageCommand(
    Guid SessionId,
    string Content,
    bool AsMe
) : ICommand<ChatMessageResponse>;

public sealed record ChatMessageResponse(
    Guid Id,
    Guid SessionId,
    string Content,
    string Sender,
    DateTime CreatedAt,
    DateTime? ReadAt
);
