using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Portfolio.Api.Extensions;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Presence;
using Portfolio.Domain.Users.Enums;

namespace Portfolio.Api.Infrastructure.Hubs;

[Authorize]
public class PresenceHub(PresenceTracker tracker, IApplicationDbContext context) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User!.GetUserId();
        if (await tracker.UserConnected(userId.ToString(), Context.ConnectionId))
        {
            await SetUserStatus(userId, UserStatus.Available);
            await Clients.Others.SendAsync("UserIsOnline", userId.ToString());
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User!.GetUserId();
        if (await tracker.UserDisconnected(userId.ToString(), Context.ConnectionId))
        {
            await SetUserStatus(userId, UserStatus.Offline);
            await Clients.Others.SendAsync("UserIsOffline", userId.ToString());
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SetMyStatus(string status)
    {
        if (Enum.TryParse<UserStatus>(status, true, out var userStatus))
        {
            var userId = Context.User!.GetUserId();
            await SetUserStatus(userId, userStatus);

            var statusString = ConvertStatusToString(userStatus);
            await Clients.Others.SendAsync("UserStatusChanged", userId.ToString(), statusString);
        }
    }

    private async Task SetUserStatus(Guid userId, UserStatus status)
    {
        var user = await context.Users.FindAsync(userId);
        if (user is not null)
        {
            user.Status = status;
            await context.SaveChangesAsync();
        }
    }

    private static string ConvertStatusToString(UserStatus status)
    {
        return status switch
        {
            UserStatus.OutOfOffice => "out-of-office",
            UserStatus.DoNotDisturb => "do-not-disturb",
            _ => status.ToString().ToLowerInvariant()
        };
    }
}