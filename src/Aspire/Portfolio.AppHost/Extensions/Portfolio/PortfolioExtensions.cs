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

        var portfolioFrontend = builder.AddNpmApp("portfolio-web", "../../Frontend/Frontend.Modules.UI/portfolio-web", "dev")
            .WithHttpEndpoint(5173, env: "VITE_PORT")
            .WithExternalHttpEndpoints()
            .WithReference(cache)
            .WithAnnotation(new ResourceRelationshipAnnotation(portfolioGroup.Resource, "Parent"))
            .WithEnvironment("VITE_GOOGLE_CLIENT_ID", "552893319584-9na3j61vqo5l2fgmobqknq9aisudhc0l.apps.googleusercontent.com")
            .WithEnvironment("VITE_LINKEDIN_CLIENT_ID", "77ri81y6q023po")
            .WithEnvironment("VITE_LINKEDIN_REDIRECT_URI", linkedInRedirect);

        var portfolioApi = builder.AddProject<Portfolio_Api>("portfolio-api")
            .WithReference(database)
            .WithReference(aiAgent)
            .WithEnvironment("PORTFOLIO_FRONTEND", portfolioFrontend.GetEndpoint("http"))
            .WithEnvironment("Auth__LinkedIn__ClientId", "77ri81y6q023po")
            .WithEnvironment("Auth__LinkedIn__ClientSecret", "WPL_AP1.mPc31DWw4Cv4VAX2.cmwlPQ==")
            .WithEnvironment("Auth__LinkedIn__RedirectUri", linkedInRedirect);

        portfolioApi.WithAnnotation(new ResourceRelationshipAnnotation(portfolioGroup.Resource, "Parent"))
            .WithUrlForEndpoint("https", url =>
            {
                url.DisplayText = "Swagger";
                url.Url = "/swagger";
            });

        portfolioFrontend.WithReference(portfolioApi);
        portfolioFrontend.WithEnvironment("VITE_PORTFOLIO_API", portfolioApi.GetEndpoint("https"));
    }
}
