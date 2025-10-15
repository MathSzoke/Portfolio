using SharedKernel;

namespace Portfolio.Domain.Chats.Events;

public sealed record ChatSessionUpdatedDomainEvent(ChatSession Session) : IDomainEvent;
