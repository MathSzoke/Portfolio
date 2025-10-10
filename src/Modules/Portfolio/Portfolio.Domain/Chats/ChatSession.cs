using Portfolio.Domain.Chats.Enums;
using SharedKernel;

namespace Portfolio.Domain.Chats;

public sealed class ChatSession : Entity
{
    public Guid Id { get; set; }
    public Guid? SenderId { get; set; }
    public Guid RecipientId { get; set; }
    public SessionStatus Status { get; set; } = SessionStatus.Open;
    public DateTime? LastSenderSeenAt { get; set; }
    public DateTime? LastRecipientSeenAt { get; set; }
    public ICollection<ChatMessage> Messages { get; set; } = [];
}