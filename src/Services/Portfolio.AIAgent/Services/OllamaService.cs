using Microsoft.SemanticKernel.ChatCompletion;
using Portfolio.AIAgent.Prompts;
using Portfolio.Application.Abstractions.AI;
using System.Text.RegularExpressions;

namespace Portfolio.AIAgent.Services;

public class OllamaService(IChatCompletionService chatService) : IAgentResponder
{
    public async Task<string> GenerateReplyAsync(IEnumerable<ChatTurn> history, string message, CancellationToken ct = default)
    {
        var trimmed = (message ?? string.Empty).Trim();
        var hasHistory = history is not null && history.Any();
        if (!hasHistory && IsGreetingPt(trimmed)) return "Olá, eu sou a IA Matheus Szoke, por agora o Matheus se encontra offline, mas posso responder por ele por agora. Um email foi encaminhado a ele informando que você entrou em contato! Com o que posso lhe ajudar!?";
        if (!hasHistory && IsGreetingEn(trimmed)) return "Hello, I’m Matheus Szoke AI. Matheus is currently offline, but I can reply on his behalf for now. An email has been forwarded to him letting him know you reached out. How can I help?";

        var chat = new ChatHistory();
        chat.AddSystemMessage(SystemPrompt.Default);

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

        chat.AddUserMessage(trimmed);

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
