using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Presence.OwnerOnline;

public sealed record OwnerOnlineQuery(string OwnerEmail) : IQuery<bool>;
