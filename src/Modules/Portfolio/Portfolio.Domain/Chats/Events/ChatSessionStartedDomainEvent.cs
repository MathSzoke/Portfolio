using SharedKernel;

namespace Portfolio.Domain.Chats.Events;

public sealed record ChatSessionStartedDomainEvent(ChatSession Session) : IDomainEvent;
