using Portfolio.Application.Abstractions.Realtime;
using Portfolio.Domain.Chats.Events;
using SharedKernel;

namespace Portfolio.Application.Chat.Sessions.Start;

internal sealed class ChatSessionStartedHandler(ISessionsNotifier notifier)
    : IDomainEventHandler<ChatSessionStartedDomainEvent>
{
    public Task Handle(ChatSessionStartedDomainEvent @event, CancellationToken ct) =>
        notifier.SessionStarted(@event.Session, ct);
}