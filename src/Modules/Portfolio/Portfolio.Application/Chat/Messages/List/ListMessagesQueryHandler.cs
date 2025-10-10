using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Chats;
using SharedKernel;

namespace Portfolio.Application.Chat.Messages.List;

internal sealed class ListMessagesQueryHandler(IApplicationDbContext db)
    : IQueryHandler<ListMessagesQuery, ChatMessagesResponse>
{
    public async Task<Result<ChatMessagesResponse>> Handle(ListMessagesQuery q, CancellationToken ct)
    {
        var exists = await db.ChatSessions.AnyAsync(s => s.Id == q.SessionId, ct);
        if (!exists) return Result.Failure<ChatMessagesResponse>(ChatErrors.SessionNotFound(q.SessionId));

        var query = db.ChatMessages.Where(m => m.SessionId == q.SessionId);
        if (q.After.HasValue) query = query.Where(m => m.CreatedAt > q.After.Value.UtcDateTime);

        var items = await query
            .OrderBy(m => m.CreatedAt)
            .Select(m => new ChatMessageItem(m.Id, m.Content, m.Sender.ToString(), m.SenderUserId, m.CreatedAt, m.ReadAt))
            .AsNoTracking()
            .ToListAsync(ct);

        return Result.Success(new ChatMessagesResponse(q.SessionId, items));
    }
}