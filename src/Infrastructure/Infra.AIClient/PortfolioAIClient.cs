using System.Net.Http.Json;
using Portfolio.Application.Abstractions.AI;

namespace Infra.AIClient;

internal sealed class PortfolioAIClient(HttpClient http) : IAgentResponder
{
    public async Task<string> GenerateReplyAsync(IEnumerable<ChatTurn> history, string message, CancellationToken ct = default)
    {
        var res = await http.PostAsJsonAsync("/api/v1/agent/reply", new { history, message }, ct);
        if (!res.IsSuccessStatusCode) return "Desculpe, estou offline no momento.";
        var body = await res.Content.ReadFromJsonAsync<ReplyResponse>(cancellationToken: ct);
        return body?.Reply ?? "Desculpe, estou offline no momento.";
    }

    private sealed record ReplyResponse(string Reply);
}
