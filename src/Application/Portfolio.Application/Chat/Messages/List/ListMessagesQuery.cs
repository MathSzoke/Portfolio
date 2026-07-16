using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Chat.Messages.List;

public sealed record ListMessagesQuery(Guid SessionId, DateTimeOffset? After) : IQuery<ChatMessagesResponse>;
public sealed record ChatMessagesResponse(Guid SessionId, IReadOnlyList<ChatMessageItem> ChatMessageItems);
public sealed record ChatMessageItem(Guid Id, string Content, string Sender, Guid? SenderUserId, DateTime CreatedAt, DateTime? ReadAt);