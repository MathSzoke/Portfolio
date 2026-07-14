using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Chat.Sessions.HasOpen;

public sealed record HasOpenSessionQuery(string Email) : IQuery<bool>;