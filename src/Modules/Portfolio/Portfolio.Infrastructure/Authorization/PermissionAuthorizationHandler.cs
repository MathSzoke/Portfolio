using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Infrastructure.Authentication;

namespace Portfolio.Infrastructure.Authorization;

internal sealed class PermissionAuthorizationHandler(IServiceScopeFactory scopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User?.Identity?.IsAuthenticated != true) return;
        if (context.User.IsInRole("SuperAdmin")) { context.Succeed(requirement); return; }
        using var scope = scopeFactory.CreateScope();
        var provider = scope.ServiceProvider.GetRequiredService<PermissionProvider>();
        var userId = context.User.GetUserId();
        var permissions = await provider.GetForUserIdAsync(userId);
        if (permissions.Contains(requirement.Permission)) context.Succeed(requirement);
    }
}