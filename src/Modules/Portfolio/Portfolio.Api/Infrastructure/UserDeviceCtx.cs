using Shared.Api.Infrastructure.User;

namespace Portfolio.Api.Infrastructure;

public static class UserDeviceCtx
{
    internal static IServiceProvider? Provider { get; set; }

    public static UserDeviceDetector UserDevice()
    {
        var accessor = Provider?.GetRequiredService<IHttpContextAccessor>();
        var req = accessor?.HttpContext?.Request ?? throw new InvalidOperationException("No HttpContext");
        var ua = req.Headers["User-Agent"].ToString();
        return UserDeviceDetector.FromUserAgent(ua);
    }
}