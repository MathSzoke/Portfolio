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

        var items = await db.ChatSessions
            .AsNoTracking()
            .Where(x => x.SenderId == target || x.RecipientId == target && x.SenderId != null)
            .OrderByDescending(x =>
                (x.LastSenderSeenAt ?? DateTime.MinValue) > (x.LastRecipientSeenAt ?? DateTime.MinValue)
                    ? (x.LastSenderSeenAt ?? DateTime.MinValue)
                    : (x.LastRecipientSeenAt ?? DateTime.MinValue))
            .ThenByDescending(x => x.CreatedAt)
            .Select(x => new ChatSessionItem(
                x.Id,
                x.SenderId,
                x.RecipientId,
                x.Status.ToString(),
                x.CreatedAt,
                x.LastSenderSeenAt,
                x.LastRecipientSeenAt))
            .ToListAsync(ct);

        return Result.Success(new ChatSessionsResponse(items));
    }
}
