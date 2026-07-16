using Projects;

namespace Portfolio.AppHost.Extensions.Portfolio;

internal static class PortfolioExtensions
{
    public static void AddPortfolioSuite(this IDistributedApplicationBuilder builder,
        IResourceBuilder<PostgresDatabaseResource> database,
        IResourceBuilder<RedisResource> cache)
    {
        var portfolioGroup = builder.AddGroup("Portfolio");

        var frontendDevUrl = "http://localhost:5173";
        var frontendProdUrl = "https://portfolio.mathszoke.com";
        var linkedInRedirectDev = $"{frontendDevUrl}/auth/callback/linkedin";
        var linkedInRedirectProd = $"{frontendProdUrl}/auth/callback/linkedin";

        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var isDev = environment.Equals("Development", StringComparison.OrdinalIgnoreCase) ||
                    environment.Equals("Local", StringComparison.OrdinalIgnoreCase);

        var linkedInRedirect = isDev ? linkedInRedirectDev : linkedInRedirectProd;

        var aiAgent = builder.AddProject<Portfolio_AIAgent>("portfolio-aiagent");

        aiAgent.WithAnnotation(new ResourceRelationshipAnnotation(portfolioGroup.Resource, "Parent"))
            .WithUrlForEndpoint("https", url =>
            {
                url.DisplayText = "Swagger";
                url.Url = "/swagger";
            });

        var portfolioApi = builder.AddProject<Portfolio_Api>("portfolio-api")
            .WithReference(database)
            .WithReference(aiAgent)
            .WithEnvironment("PORTFOLIO_FRONTEND", isDev ? frontendDevUrl : frontendProdUrl)
            .WithEnvironment("Auth__LinkedIn__ClientId", "77ri81y6q023po")
            .WithEnvironment("Auth__LinkedIn__ClientSecret", "WPL_AP1.mPc31DWw4Cv4VAX2.cmwlPQ==")
            .WithEnvironment("Auth__LinkedIn__RedirectUri", linkedInRedirect);

        portfolioApi.WithAnnotation(new ResourceRelationshipAnnotation(portfolioGroup.Resource, "Parent"))
            .WithUrlForEndpoint("https", url =>
            {
                url.DisplayText = "Swagger";
                url.Url = "/swagger";
            });

    }
}
