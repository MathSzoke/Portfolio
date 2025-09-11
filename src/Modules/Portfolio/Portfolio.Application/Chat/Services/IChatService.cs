using Portfolio.Application.Chat.Messages.Post;
using Portfolio.Domain.Chats;

namespace Portfolio.Application.Chat.Services;

public interface IChatService
{
    Task<ChatMessage> SendMessageAsync(PostMessageCommand command, CancellationToken ct = default);
    Task<List<ChatMessage>> GetChatHistoryAsync(Guid sessionId, CancellationToken ct = default);
}