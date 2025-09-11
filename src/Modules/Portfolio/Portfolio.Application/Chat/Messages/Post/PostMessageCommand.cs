using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Chats.Enums;

namespace Portfolio.Application.Chat.Messages.Post;

public sealed record PostMessageCommand(Guid SessionId, Sender Sender, string Content, bool AsMe) : ICommand<ChatMessageResponse>;
public sealed record ChatMessageResponse(Guid Id, Guid SessionId, string Content, string Sender, DateTime CreatedAt, DateTime? ReadAt);