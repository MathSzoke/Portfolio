using SharedKernel;

namespace Portfolio.Domain.Chats.Events;

public sealed record ChatMessageReadDomainEvent(ChatMessage Message) : IDomainEvent;
