using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Chat.Sessions.List;

public sealed record ListSessionsQuery(Guid? UserId) : IQuery<ChatSessionsResponse>;
public sealed record ChatSessionsResponse(IReadOnlyList<ChatSessionItem> Items);
public sealed record ChatSessionItem(
    Guid Id,
    Guid? SenderId,
    Guid RecipientId,
    string Status,
    DateTime CreatedAt,
    DateTime? LastSenderSeenAt,
    DateTime? LastRecipientSeenAt
);
