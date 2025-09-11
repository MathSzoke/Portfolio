using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Chats;
using SharedKernel;

namespace Portfolio.Application.Chat.Messages.Read;

internal sealed class MarkMessageReadCommandHandler(IApplicationDbContext db) : ICommandHandler<MarkMessageReadCommand>
{
    public async Task<Result> Handle(MarkMessageReadCommand cmd, CancellationToken ct)
    {
        var m = await db.ChatMessages.FirstOrDefaultAsync(x => x.Id == cmd.MessageId, ct);
        if (m is null) return Result.Failure(ChatErrors.MessageNotFound(cmd.MessageId));
        m.ReadAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}