using Microsoft.AspNetCore.SignalR;
using Portfolio.Api.Infrastructure.Hubs;
using Portfolio.Application.Abstractions.Realtime;
using Portfolio.Domain.Chats;

namespace Portfolio.Api.Infrastructure.Realtime;

internal sealed class SignalRSessionsNotifier(IHubContext<SessionsHub> hub) : ISessionsNotifier
{
    public Task SessionStarted(ChatSession session, CancellationToken ct = default) =>
        this.ToUsers(session).SendAsync("SessionStarted", ToPayload(session), ct);

    public Task SessionUpdated(ChatSession session, CancellationToken ct = default) =>
        this.ToUsers(session).SendAsync("SessionUpdated", ToPayload(session), ct);

    public Task SessionClosed(ChatSession session, CancellationToken ct = default) =>
        this.ToUsers(session).SendAsync("SessionClosed", new { id = session.Id.ToString() }, ct);

    private IClientProxy ToUsers(ChatSession s) =>
        hub.Clients.Users(s.SenderId.ToString()!, s.RecipientId.ToString());

    private static object ToPayload(ChatSession s) => new
    {
        id = s.Id.ToString(),
        senderId = s.SenderId.ToString(),
        recipientId = s.RecipientId.ToString(),
        status = s.Status.ToString(),
        createdAt = s.CreatedAt,
        lastSenderSeenAt = s.LastSenderSeenAt,
        lastRecipientSeenAt = s.LastRecipientSeenAt,
        v = 1
    };
}
