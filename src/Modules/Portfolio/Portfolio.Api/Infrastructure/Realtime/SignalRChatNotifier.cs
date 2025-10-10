using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Portfolio.Api.Infrastructure.Hubs;
using Portfolio.Application.Abstractions.Realtime;

namespace Portfolio.Api.Infrastructure.Realtime;

public sealed class SignalRChatNotifier(IHubContext<PresenceHub> hub) : IChatNotifier
{
    public Task SendMessageToSessionAsync(Guid sessionId, Guid messageId, string content, string sender, Guid? senderUserId, DateTime createdAt, DateTime? readAt, CancellationToken ct = default)
    {
        var payload = new
        {
            id = messageId,
            sessionId,
            content,
            sender,
            senderUserId,
            createdAt,
            readAt
        };
        return hub.Clients.Group(sessionId.ToString()).SendAsync("message", payload, ct);
    }
}
