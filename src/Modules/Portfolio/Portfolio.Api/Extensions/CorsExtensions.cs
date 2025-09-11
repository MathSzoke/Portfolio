namespace Portfolio.Api.Extensions;

public static class CorsExtensions
{
    private const string CorsPolicyName = "AllowMyFrontend";
    public static IServiceCollection AddCorsServices(this IServiceCollection services, IConfiguration configuration)
    {
        var frontendUrl = configuration["PORTFOLIO_FRONTEND"]!;

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy =>
            {
                policy.WithOrigins(frontendUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        return services;
    }

    public static IApplicationBuilder UseCorsApp(this IApplicationBuilder app)
    {
        app.UseCors(CorsPolicyName);
        return app;
    }
}