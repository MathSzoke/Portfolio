using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Chat.Messages.Post;
using Portfolio.Domain.Chats;

namespace Portfolio.Application.Chat.Services;


public class ChatService(IApplicationDbContext db) : IChatService
{
    public async Task<ChatMessage> SendMessageAsync(PostMessageCommand command, CancellationToken ct = default)
    {
        var message = new ChatMessage()
        {
            SessionId = command.SessionId,
            Sender = command.Sender,
            Content = command.Content
            
        };
        db.ChatMessages.Add(message);
        await db.SaveChangesAsync(ct);

        return message;
    }

    public async Task<List<ChatMessage>> GetChatHistoryAsync(Guid sessionId, CancellationToken ct = default)
    {
        return await db.ChatMessages
            .Where(x => x.SessionId == sessionId)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(ct);
    }
}