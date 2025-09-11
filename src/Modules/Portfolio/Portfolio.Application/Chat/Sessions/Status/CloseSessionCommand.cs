using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Chat.Sessions.Status;

public sealed record CloseSessionCommand(Guid SessionId) : ICommand;
public sealed record ArchiveSessionCommand(Guid SessionId) : ICommand;
public sealed record ReopenSessionCommand(Guid SessionId) : ICommand;