using Portfolio.Application.Chat.Sessions.Start;
using Portfolio.Domain.Chats;

namespace Portfolio.Application.Abstractions.Realtime;

public interface ISessionsNotifier
{
    Task SessionStarted(ChatSession session, CancellationToken ct = default);
    Task SessionUpdated(ChatSession session, CancellationToken ct = default);
    Task SessionClosed(ChatSession session, CancellationToken ct = default);
}