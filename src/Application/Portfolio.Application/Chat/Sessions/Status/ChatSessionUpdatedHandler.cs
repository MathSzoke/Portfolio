using Portfolio.Application.Abstractions.Realtime;
using Portfolio.Domain.Chats.Events;
using SharedKernel;

namespace Portfolio.Application.Chat.Sessions.Status;

internal sealed class ChatSessionUpdatedHandler(ISessionsNotifier notifier)
    : IDomainEventHandler<ChatSessionUpdatedDomainEvent>
{
    public Task Handle(ChatSessionUpdatedDomainEvent @event, CancellationToken ct) =>
        notifier.SessionUpdated(@event.Session, ct);
}