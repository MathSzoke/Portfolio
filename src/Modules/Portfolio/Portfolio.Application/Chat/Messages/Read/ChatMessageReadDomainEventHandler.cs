using Portfolio.Application.Abstractions.Realtime;
using Portfolio.Domain.Chats.Events;
using SharedKernel;

namespace Portfolio.Application.Chat.Messages.Read;

internal sealed class ChatMessageReadDomainEventHandler(IChatNotifier notifier)
    : IDomainEventHandler<ChatMessageReadDomainEvent>
{
    public Task Handle(ChatMessageReadDomainEvent @event, CancellationToken ct) =>
        notifier.SendMessageReadAsync(
            @event.Message.SessionId,
            @event.Message.Id,
            @event.Message.SenderUserId,
            @event.Message.ReadAt!.Value,
            ct);
}