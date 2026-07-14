using Portfolio.Domain.Chats.Enums;
using SharedKernel;

namespace Portfolio.Domain.Chats;

public sealed class EmailOutbox : Entity
{
    public Guid Id { get; set; }

    public Guid? SessionId { get; set; }
    public ChatSession? Session { get; set; }

    public string ToEmail { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Body { get; set; } = null!;

    public EmailStatus Status { get; set; } = EmailStatus.Pending;

    public DateTime? SentAt { get; set; }
    public string? Error { get; set; }
}