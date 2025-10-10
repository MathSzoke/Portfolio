using System.Net.Http.Json;
using Portfolio.Application.Abstractions.AI;

namespace Infra.AIClient;

internal sealed class PortfolioAIClient(HttpClient http) : IAgentResponder
{
    public async Task<string> GenerateReplyAsync(Guid sessionId, string name, string lastMessage)
    {
        var res = await http.PostAsJsonAsync("/api/v1/agent/reply", new { sessionId, name, lastMessage });
        if (!res.IsSuccessStatusCode) return "Desculpe, estou offline no momento.";
        var body = await res.Content.ReadFromJsonAsync<ReplyResponse>();
        return body?.Reply ?? "Desculpe, estou offline no momento.";
    }

    private sealed record ReplyResponse(string Reply);
}