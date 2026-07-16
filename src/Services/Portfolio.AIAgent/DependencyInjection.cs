using Microsoft.SemanticKernel;
using Portfolio.AIAgent.Services;
using Portfolio.Application.Abstractions.AI;

namespace Portfolio.AIAgent;

public static class DependencyInjection
{
    public static IServiceCollection AddAiAgent(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        if (environment.IsEnvironment("Testing"))
        {
            services.AddScoped<IAgentResponder, OfflineAgentResponder>();
            return services;
        }

        var apiKey = configuration["OpenAI:ApiKey"] ?? configuration["OPENAI_API_KEY"] ?? string.Empty;
        var modelId = configuration["OpenAI:ModelId"] ?? "gpt-5-nano";

        services.AddOpenAIChatCompletion(modelId, apiKey);
        services.AddScoped<IAgentResponder, GptService>();
        return services;
    }

    private sealed class OfflineAgentResponder : IAgentResponder
    {
        public Task<string> GenerateReplyAsync(IEnumerable<ChatTurn> history, string message, CancellationToken ct = default)
        {
            return Task.FromResult("AI agent is disabled for tests.");
        }
    }
}
