using Portfolio.Application.Abstractions.Realtime;
using Portfolio.Domain.Chats.Events;
using SharedKernel;

namespace Portfolio.Application.Chat.Sessions.Status;

internal sealed class ChatSessionClosedHandler(ISessionsNotifier notifier)
    : IDomainEventHandler<ChatSessionClosedDomainEvent>
{
    public Task Handle(ChatSessionClosedDomainEvent @event, CancellationToken ct) =>
        notifier.SessionClosed(
            @event.Session,
            ct);
}