using Portfolio.Api.Infrastructure.Hubs;

namespace Portfolio.Api.Extensions;

public static class MapHubsExtension
{
    public static IEndpointRouteBuilder MapHubs(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHub<PresenceHub>("/hubs/presence");
        endpoints.MapHub<SessionsHub>("/hubs/sessions");
        endpoints.MapHub<ChatHub>("/hubs/chat");

        return endpoints;
    }
}
