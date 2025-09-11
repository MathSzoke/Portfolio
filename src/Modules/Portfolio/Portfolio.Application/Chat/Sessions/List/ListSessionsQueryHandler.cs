using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Chats.Enums;
using SharedKernel;

namespace Portfolio.Application.Chat.Sessions.List;

internal sealed class ListSessionsQueryHandler(IApplicationDbContext db)
    : IQueryHandler<ListSessionsQuery, ChatSessionsPageResponse>
{
    public async Task<Result<ChatSessionsPageResponse>> Handle(ListSessionsQuery q, CancellationToken ct)
    {
        var query = db.ChatSessions.AsQueryable();

        if (!string.IsNullOrWhiteSpace(q.Status) && Enum.TryParse<SessionStatus>(q.Status, true, out var s))
            query = query.Where(x => x.Status == s);

        if (!string.IsNullOrWhiteSpace(q.Search))
        {
            var t = q.Search.Trim();
            query = query.Where(x => (x.Name ?? "").Contains(t) || (x.Email ?? "").Contains(t));
        }

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(x => x.LastSeenAt ?? x.UpdatedAt)
            .ThenByDescending(x => x.CreatedAt)
            .Skip((q.Page - 1) * q.PageSize)
            .Take(q.PageSize)
            .Select(x => new ChatSessionItem(x.Id, x.Name, x.Email, x.Status.ToString(), x.CreatedAt, x.LastSeenAt))
            .AsNoTracking()
            .ToListAsync(ct);

        return Result.Success(new ChatSessionsPageResponse(q.Page, q.PageSize, total, items));
    }
}