namespace Portfolio.Application.Abstractions.AI;

public record ChatTurn(string Role, string Content);

public interface IAgentResponder
{
    Task<string> GenerateReplyAsync(IEnumerable<ChatTurn> history, string message, CancellationToken ct = default);
}
