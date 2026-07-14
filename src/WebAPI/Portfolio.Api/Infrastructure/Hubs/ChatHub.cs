using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Portfolio.Api.Infrastructure.Hubs;

public sealed class ChatHub : Hub
{
    public Task JoinSession(string sessionId) =>
        Groups.AddToGroupAsync(Context.ConnectionId, sessionId);

    public Task LeaveSession(string sessionId) =>
        Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);

    public override Task OnDisconnectedAsync(Exception? ex) => base.OnDisconnectedAsync(ex);
}