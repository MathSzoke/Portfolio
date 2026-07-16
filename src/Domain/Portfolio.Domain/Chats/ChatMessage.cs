using Portfolio.Domain.Chats.Enums;
using Portfolio.Domain.Chats.Events;
using SharedKernel;
using System.ComponentModel.DataAnnotations;

namespace Portfolio.Domain.Chats;

public sealed class ChatMessage : Entity
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public Sender Sender { get; set; } = Sender.Visitor;
    public Guid? SenderUserId { get; set; }
    [MaxLength(3000)]
    public string Content { get; set; } = null!;
    public bool Delivered { get; set; }
    public DateTime? ReadAt { get; set; }
    public ChatSession Session { get; set; } = null!;

    public static ChatMessage Create(Guid sessionId, string content, Sender sender, Guid? senderUserId)
    {
        var m = new ChatMessage
        {
            Id = Guid.NewGuid(),
            SessionId = sessionId,
            Content = content,
            Sender = sender,
            SenderUserId = senderUserId
        };
        m.Raise(new ChatMessagePostedDomainEvent(m));
        return m;
    }

    public void MarkRead()
    {
        if (ReadAt is not null) return;
        ReadAt = DateTime.UtcNow;
        this.Raise(new ChatMessageReadDomainEvent(this));
    }
}
