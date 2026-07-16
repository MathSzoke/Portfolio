using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Chats;
using SharedKernel;

namespace Portfolio.Application.Chat.Sessions.List;

internal sealed class ListSessionsQueryHandler(
    IApplicationDbContext db,
    ICurrentUserContext current
) : IQueryHandler<ListSessionsQuery, ChatSessionsResponse>
{
    public async Task<Result<ChatSessionsResponse>> Handle(ListSessionsQuery q, CancellationToken ct)
    {
        var ctxUser = current.UserIdGuid;
        if (ctxUser == Guid.Empty) return Result.Failure<ChatSessionsResponse>(ChatErrors.Unauthorized);

        var target = q.UserId.HasValue && q.UserId.Value != Guid.Empty ? q.UserId.Value : ctxUser;

        var rows = await db.ChatSessions
            .AsNoTracking()
            .Where(x => (x.SenderId == target || x.RecipientId == target) && x.SenderId != null)
            .Select(x => new SessionRow(
                x.Id,
                x.SenderId,
                x.RecipientId,
                x.Status.ToString(),
                x.CreatedAt,
                x.LastSenderSeenAt,
                x.LastRecipientSeenAt,
                x.Messages.Any()))
            .ToListAsync(ct);

        var items = rows
            .GroupBy(x => ConversationKey(x.SenderId, x.RecipientId))
            .Select(g => g
                .OrderByDescending(x => x.HasMessages)
                .ThenByDescending(x => LastActivity(x))
                .ThenByDescending(x => x.CreatedAt)
                .First())
            .OrderByDescending(LastActivity)
            .ThenByDescending(x => x.CreatedAt)
            .Select(x => new ChatSessionItem(
                x.Id,
                x.SenderId,
                x.RecipientId,
                x.Status,
                x.CreatedAt,
                x.LastSenderSeenAt,
                x.LastRecipientSeenAt))
            .ToList();

        return Result.Success(new ChatSessionsResponse(items));
    }

    private static string ConversationKey(Guid? senderId, Guid recipientId)
    {
        var first = senderId ?? Guid.Empty;
        var second = recipientId;
        return first.CompareTo(second) <= 0 ? $"{first:N}:{second:N}" : $"{second:N}:{first:N}";
    }

    private static DateTime LastActivity(SessionRow session)
    {
        var senderSeen = session.LastSenderSeenAt ?? DateTime.MinValue;
        var recipientSeen = session.LastRecipientSeenAt ?? DateTime.MinValue;
        return senderSeen > recipientSeen ? senderSeen : recipientSeen;
    }

    private sealed record SessionRow(
        Guid Id,
        Guid? SenderId,
        Guid RecipientId,
        string Status,
        DateTime CreatedAt,
        DateTime? LastSenderSeenAt,
        DateTime? LastRecipientSeenAt,
        bool HasMessages);
}
