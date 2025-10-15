using Portfolio.Application.Abstractions.Realtime;
using Portfolio.Domain.Chats.Events;
using SharedKernel;

namespace Portfolio.Application.Chat.Messages.Post;

internal class ChatMessagePostedDomainEventHandler(IChatNotifier notifier)
    : IDomainEventHandler<ChatMessagePostedDomainEvent>
{
    public Task Handle(ChatMessagePostedDomainEvent @event, CancellationToken ct) =>
        notifier.SendMessageToSessionAsync(
            @event.Message.SessionId,
            @event.Message.Id,
            @event.Message.Content,
            @event.Message.Sender.ToString(),
            @event.Message.SenderUserId,
            @event.Message.CreatedAt,
            @event.Message.ReadAt,
            ct);
}
