using System.ComponentModel.DataAnnotations;
using Portfolio.Domain.Chats.Enums;
using SharedKernel;

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
}
