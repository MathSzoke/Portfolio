namespace Portfolio.Application.Abstractions.Realtime;

public interface IChatNotifier
{
    Task SendMessageToSessionAsync(
        Guid sessionId, Guid messageId, string content, string sender, Guid? senderUserId,
        DateTime createdAt, DateTime? readAt, CancellationToken ct = default);

    Task SendMessageReadAsync(
        Guid sessionId, Guid messageId, Guid? readerUserId, DateTime readAt,
        CancellationToken ct = default);
}
