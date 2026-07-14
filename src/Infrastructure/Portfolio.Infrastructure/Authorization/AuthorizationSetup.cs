using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;

namespace Portfolio.Infrastructure.Authorization;

public static class AuthorizationSetup
{
    public static IServiceCollection AddPortfolioAuthorization(this IServiceCollection services)
    {
        services.AddScoped<PermissionProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddAuthorizationBuilder()
            .AddPolicy("SuperAdminOnly", p => p.RequireAuthenticatedUser().Requirements.Add(new PermissionRequirement("SuperAdmin")));
        return services;
    }
}