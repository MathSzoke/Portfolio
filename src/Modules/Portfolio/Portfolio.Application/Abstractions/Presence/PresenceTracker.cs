namespace Portfolio.Application.Abstractions.Presence;

public abstract class PresenceTracker
{
    private readonly Dictionary<string, List<string>> _onlineUsers = [];

    public Task<bool> UserConnected(string userId, string connectionId)
    {
        var isOnline = false;
        lock (_onlineUsers)
        {
            if (_onlineUsers.TryGetValue(userId, out var value))
            {
                value.Add(connectionId);
            }
            else
            {
                _onlineUsers.Add(userId, [connectionId]);
                isOnline = true;
            }
        }

        return Task.FromResult(isOnline);
    }

    public Task<bool> UserDisconnected(string userId, string connectionId)
    {
        var isOffline = false;
        lock (_onlineUsers)
        {
            if (!_onlineUsers.TryGetValue(userId, out var value)) return Task.FromResult(isOffline);
            value.Remove(connectionId);

            if (value.Count != 0) return Task.FromResult(isOffline);
            _onlineUsers.Remove(userId);
            isOffline = true;
        }

        return Task.FromResult(isOffline);
    }

    public Task<bool> IsOnline(string userId)
    {
        lock (_onlineUsers)
        {
            return Task.FromResult(_onlineUsers.ContainsKey(userId));
        }
    }
}