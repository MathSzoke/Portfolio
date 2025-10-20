using Microsoft.SemanticKernel.ChatCompletion;
using Portfolio.AIAgent.Prompts;
using Portfolio.Application.Abstractions.AI;
using System.Text.RegularExpressions;

namespace Portfolio.AIAgent.Services;

public class GptService(IChatCompletionService chatService) : IAgentResponder
{
    public async Task<string> GenerateReplyAsync(IEnumerable<ChatTurn> history, string message, CancellationToken ct = default)
    {
        var trimmed = (message ?? string.Empty).Trim();
        var hasHistory = history is not null && history.Any();

        if (!hasHistory && IsGreetingPt(trimmed)) return "Eu sou a IA Matheus Szoke. O Matheus está offline no momento.";
        if (!hasHistory && IsGreetingEn(trimmed)) return "I’m Matheus Szoke AI. Matheus is currently offline.";

        var chat = new ChatHistory();
        chat.AddSystemMessage(SystemPrompt.SYSTEM);

        if (hasHistory)
        {
            foreach (var t in history!)
            {
                if (string.Equals(t.Role, "user", StringComparison.OrdinalIgnoreCase))
                    chat.AddUserMessage(t.Content);
                else
                    chat.AddAssistantMessage(t.Content);
            }
        }

        chat.AddUserMessage(SystemPrompt.BuildUserPrompt(trimmed));

        var result = await chatService.GetChatMessageContentAsync(chat, cancellationToken: ct);
        return result.Content?.Trim() ?? string.Empty;
    }

    private static bool IsGreetingPt(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        var patterns = new[]
        {
            @"^(olá|ola|oi|e?ai|e\s*a[íi])\.?$",
            @"^(boa\s*noite|boa\s*tarde|bom\s*dia)\b"
        };
        return patterns.Any(p => Regex.IsMatch(input, p, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));
    }

    private static bool IsGreetingEn(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        var patterns = new[]
        {
            @"^(hi|hello|hey|yo|sup)\b\.?$"
        };
        return patterns.Any(p => Regex.IsMatch(input, p, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));
    }
}
