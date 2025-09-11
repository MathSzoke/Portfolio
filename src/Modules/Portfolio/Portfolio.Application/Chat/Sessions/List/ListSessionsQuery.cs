using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Chat.Sessions.List;

public sealed record ListSessionsQuery(string? Status, string? Search, int Page, int PageSize) : IQuery<ChatSessionsPageResponse>;
public sealed record ChatSessionsPageResponse(int Page, int PageSize, int Total, IReadOnlyList<ChatSessionItem> ChatSessionItems);
public sealed record ChatSessionItem(Guid Id, string? Name, string? Email, string Status, DateTime CreatedAt, DateTime? LastSeenAt);