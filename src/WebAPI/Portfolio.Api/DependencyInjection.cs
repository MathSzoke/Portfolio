using Infra.Database.Portfolio;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Api.Infrastructure.Realtime;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Realtime;
using Portfolio.Infrastructure.Authorization;
using Serilog;
using Shared.Api.Infrastructure.User;
using SharedKernel;

namespace Portfolio.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer()
            .AddPortfolioAuthorization()
            .AddControllers();

        services.AddSwaggerGenWithAuth();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddHttpClient();
        services.AddHttpContextAccessor();
        services.AddServices();

        services.AddUserDeviceDetection();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddDatabase();
        services.AddSignalR();
        services.AddSingleton<ICurrentUserContext, CurrentUserContext>();
        services.AddScoped<ISessionsNotifier, SignalRSessionsNotifier>();
        services.AddScoped<IChatNotifier, SignalRChatNotifier>();
        return services;
    }

    public static IApplicationBuilder ApplicationUses(this IApplicationBuilder app)
    {
        var supportedCultures = new[] { "en-US", "pt-BR" };
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);

        app.UseHttpsRedirection();
        app.UseRequestLocalization(localizationOptions);
        app.UseRequestContextLogging();
        app.UseSerilogRequestLogging();
        app.UseExceptionHandler();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    public static IServiceCollection AddUserDeviceDetection(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        return services;
    }
}
