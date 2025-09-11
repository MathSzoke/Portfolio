using Portfolio.Domain.Chats.Enums;
using SharedKernel;

namespace Portfolio.Domain.Chats;

public sealed class ChatSession : Entity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public SessionStatus Status { get; set; } = SessionStatus.Open;
    public string? ClientIp { get; set; }
    public string? UserAgent { get; set; }
    public bool ConsentEmail { get; set; } = true;
    public DateTime? LastSeenAt { get; set; }
    public DateTime? LastAgentSeenAt { get; set; }
    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}