using SharedKernel;

namespace Portfolio.Domain.Chats;

public static class ChatErrors
{
    public static Error SessionNotFound(Guid sessionId) =>
        Error.NotFound("Chat.SessionNotFound", $"The chat session with Id = '{sessionId}' was not found");

    public static readonly Error SessionAlreadyClosed =
        Error.Conflict("Chat.SessionAlreadyClosed", "The chat session is already closed");

    public static readonly Error SessionAlreadyArchived =
        Error.Conflict("Chat.SessionAlreadyArchived", "The chat session is already archived");

    public static readonly Error SessionStatusInvalid =
        Error.Failure("Chat.SessionStatusInvalid", "The chat session status is invalid");

    public static readonly Error NameRequired =
        Error.Failure("Chat.NameRequired", "The visitor name is required");

    public static readonly Error EmailRequired =
        Error.Failure("Chat.EmailRequired", "The visitor email is required");

    public static readonly Error EmailInvalid =
        Error.Failure("Chat.EmailInvalid", "The visitor email is invalid");

    public static Error MessageNotFound(Guid messageId) =>
        Error.NotFound("Chat.MessageNotFound", $"The chat message with Id = '{messageId}' was not found");

    public static readonly Error MessageEmpty =
        Error.Failure("Chat.MessageEmpty", "The message content cannot be empty");

    public static readonly Error MessageTooLong =
        Error.Failure("Chat.MessageTooLong", "The message content is too long");

    public static readonly Error MessageNotAllowedInClosedSession =
        Error.Conflict("Chat.MessageNotAllowedInClosedSession", "Cannot send messages to a closed or archived session");

    public static readonly Error SenderInvalid =
        Error.Failure("Chat.SenderInvalid", "The message sender is invalid");

    public static Error OutboxNotFound(Guid outboxId) =>
        Error.NotFound("Chat.OutboxNotFound", $"The email outbox item with Id = '{outboxId}' was not found");

    public static readonly Error OutboxAlreadySent =
        Error.Conflict("Chat.OutboxAlreadySent", "The email outbox item has already been sent");

    public static readonly Error OutboxStatusInvalid =
        Error.Failure("Chat.OutboxStatusInvalid", "The email outbox status is invalid");

    public static readonly Error RateLimited =
        Error.Conflict("Chat.RateLimited", "Too many requests. Please try again later.");

    public static readonly Error Unauthorized =
        Error.Failure("Chat.Unauthorized", "You are not authorized to perform this action.");

    public static readonly Error Forbidden =
        Error.Failure("Chat.Forbidden", "You do not have permission to perform this action.");

    public static readonly Error ConcurrencyConflict =
        Error.Conflict("Chat.ConcurrencyConflict", "The resource was modified by another process.");

    public static readonly Error InvalidPayload =
        Error.Failure("Chat.InvalidPayload", "The request payload is invalid");
}
