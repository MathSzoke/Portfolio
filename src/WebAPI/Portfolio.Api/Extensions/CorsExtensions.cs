namespace Portfolio.Api.Extensions;

public static class CorsExtensions
{
    private const string CorsPolicyName = "AllowMyFrontend";
    public static IServiceCollection AddCorsServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        var frontendUrl = configuration["PORTFOLIO_FRONTEND"];
        if (string.IsNullOrWhiteSpace(frontendUrl) && environment.IsEnvironment("Testing"))
        {
            frontendUrl = "http://localhost";
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(frontendUrl);

        services.AddCors(options => options.AddPolicy(CorsPolicyName, policy => policy.WithOrigins(frontendUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()));

        return services;
    }

    public static IApplicationBuilder UseCorsApp(this IApplicationBuilder app)
    {
        app.UseCors(CorsPolicyName);
        return app;
    }
}
