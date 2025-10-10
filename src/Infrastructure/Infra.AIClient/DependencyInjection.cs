using Microsoft.Extensions.DependencyInjection;
using Portfolio.Application.Abstractions.AI;

namespace Infra.AIClient;

public static class DependencyInjection
{
    public static IServiceCollection AddAIClient(this IServiceCollection services, string baseUrl)
    {
        services.AddHttpClient<IAgentResponder, PortfolioAIClient>(c =>
        {
            c.BaseAddress = new Uri(baseUrl);
            c.Timeout = TimeSpan.FromSeconds(15);
        });
        return services;
    }
}
