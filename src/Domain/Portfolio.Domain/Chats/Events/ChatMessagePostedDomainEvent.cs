using SharedKernel;

namespace Portfolio.Domain.Chats.Events;

public sealed record ChatMessagePostedDomainEvent(ChatMessage Message) : IDomainEvent;