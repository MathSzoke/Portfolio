using Microsoft.SemanticKernel;
using Portfolio.AIAgent.Services;
using Portfolio.Application.Abstractions.AI;

namespace Portfolio.AIAgent;

public static class DependencyInjection
{
    public static IServiceCollection AddAiAgent(this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration.GetValue<string>("OllamaUrl") ?? "http://localhost:11434";

        services.AddOllamaChatCompletion(
            modelId: "phi3.5",
            endpoint: new Uri(url)
        );

        services.AddScoped<IAgentResponder, OllamaService>();
        return services;
    }
}
