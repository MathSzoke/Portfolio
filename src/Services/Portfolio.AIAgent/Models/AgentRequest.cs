namespace Portfolio.AIAgent.Models;

public sealed record ChatTurnDto(string Role, string Content);

public class AgentRequest
{
    public string Message { get; set; } = string.Empty;
    public List<ChatTurnDto>? History { get; set; }
}
