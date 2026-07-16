using SharedKernel;

namespace Portfolio.Domain.Chats.Events;

public sealed record ChatSessionClosedDomainEvent(ChatSession Session) : IDomainEvent;
