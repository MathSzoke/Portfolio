using DeviceDetectorNET;
using DeviceDetectorNET.Parser;

namespace Shared.Api.Infrastructure.User;

public sealed class UserDeviceDetector(string? userAgent)
{
    private readonly string _ua = userAgent ?? string.Empty;

    public static UserDeviceDetector FromUserAgent(string userAgent) => new(userAgent);

    public DeviceDetectionResult Find()
    {
        DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);
        var dd = new DeviceDetector(_ua);
        dd.Parse();

        var isBot = dd.IsBot();
        var deviceName = dd.GetDeviceName() ?? "desktop";
        var normalized = Normalize(deviceName);
        var client = dd.GetClient()?.Match?.Name;
        var os = dd.GetOs()?.Match?.Name;
        var brand = dd.GetBrandName();
        var model = dd.GetModel();

        return new DeviceDetectionResult(
            normalized,
            deviceName,
            client,
            os,
            brand,
            model,
            _ua,
            isBot
        );
    }

    private static string Normalize(string deviceName)
    {
        var n = deviceName.ToLowerInvariant();
        if (n.Contains("smartphone") || n.Contains("phone") || n.Contains("tablet") || n.Contains("phablet"))
            return "mobile";
        return "desktop";
    }
}

public sealed record DeviceDetectionResult(
    string Device,
    string DeviceName,
    string? Client,
    string? Os,
    string? Brand,
    string? Model,
    string UserAgent,
    bool IsBot
);