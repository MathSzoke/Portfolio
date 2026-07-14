using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Portfolio.Api.Infrastructure.Hubs;

public sealed class PresenceHub : Hub
{
    public Task JoinSession(string sessionId) => Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
    public Task LeaveSession(string sessionId) => Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);
}
