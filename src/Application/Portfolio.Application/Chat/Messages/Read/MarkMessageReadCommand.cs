using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Chat.Messages.Read;

public sealed record MarkMessageReadCommand(Guid MessageId) : ICommand;