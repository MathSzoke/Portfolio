using Microsoft.AspNetCore.SignalR;
using Portfolio.Api.Infrastructure.Hubs;
using Portfolio.Application.Abstractions.Realtime;

namespace Portfolio.Api.Infrastructure.Realtime;

public sealed class SignalRChatNotifier(IHubContext<ChatHub> hub) : IChatNotifier
{
    public Task SendMessageToSessionAsync(Guid sessionId, Guid messageId, string content, string sender,
        Guid? senderUserId, DateTime createdAt, DateTime? readAt, CancellationToken ct = default)
    {
        var payload = new { id = messageId, sessionId, content, sender, senderUserId, createdAt, readAt };
        return hub.Clients.Group(sessionId.ToString()).SendAsync("MessagePosted", payload, ct);
    }

    public Task SendMessageReadAsync(Guid sessionId, Guid messageId, Guid? readerUserId, DateTime readAt, CancellationToken ct = default)
    {
        var payload = new { sessionId, messageId, readerUserId, readAt };
        return hub.Clients.Group(sessionId.ToString()).SendAsync("MessageRead", payload, ct);
    }
}