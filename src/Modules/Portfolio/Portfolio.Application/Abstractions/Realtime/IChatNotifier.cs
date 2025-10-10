using System;
using System.Threading;
using System.Threading.Tasks;

namespace Portfolio.Application.Abstractions.Realtime;

public interface IChatNotifier
{
    Task SendMessageToSessionAsync(Guid sessionId, Guid messageId, string content, string sender, DateTime createdAt, DateTime? readAt, CancellationToken ct = default);
}
