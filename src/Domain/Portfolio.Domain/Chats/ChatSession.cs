using Portfolio.Domain.Chats.Enums;
using Portfolio.Domain.Chats.Events;
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


    public void Close()
    {
        if (Status == SessionStatus.Closed) return;
        Status = SessionStatus.Closed;
        this.Raise(new ChatSessionClosedDomainEvent(this));
    }

    public void Archive()
    {
        if (Status == SessionStatus.Archived) return;
        Status = SessionStatus.Archived;
        this.Raise(new ChatSessionUpdatedDomainEvent(this));
    }

    public void Reopen()
    {
        if (Status == SessionStatus.Open) return;
        Status = SessionStatus.Open;
        this.Raise(new ChatSessionUpdatedDomainEvent(this));
    }

    public static ChatSession Start(Guid id, Guid senderId, Guid recipientId)
    {
        var s = new ChatSession
        {
            Id = id,
            SenderId = senderId,
            RecipientId = recipientId,
            Status = SessionStatus.Open
        };
        s.Raise(new ChatSessionStartedDomainEvent(s));
        return s;
    }
}