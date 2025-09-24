namespace Portfolio.Api.Contracts.Auth;

public static class RefreshTokenContract
{
    public static void SetRefreshCookie(HttpContext http, IConfiguration cfg, string refreshToken, DateTime expiresUtc)
    {
        var name = cfg["Jwt:CookieName"];
        if (string.IsNullOrWhiteSpace(name)) name = "__Host-rt";
        var domain = cfg["Jwt:CookieDomain"];
        var secure = cfg.GetValue<bool?>("Jwt:CookieSecure") ?? true;
        var sameSiteStr = cfg["Jwt:CookieSameSite"] ?? "None";
        var sameSite = sameSiteStr.Equals("None", StringComparison.OrdinalIgnoreCase) ? SameSiteMode.None :
                         sameSiteStr.Equals("Strict", StringComparison.OrdinalIgnoreCase) ? SameSiteMode.Strict :
                         SameSiteMode.Lax;

        var opts = new CookieOptions
        {
            HttpOnly = true,
            Secure = secure,
            SameSite = sameSite,
            Expires = new DateTimeOffset(expiresUtc, TimeSpan.Zero),
            Path = "/",
            IsEssential = true
        };

        if (!name.StartsWith("__Host-") && !string.IsNullOrWhiteSpace(domain))
            opts.Domain = domain;

        http.Response.Cookies.Append(name, refreshToken, opts);
    }

    public static void ClearRefreshCookie(HttpContext http, IConfiguration cfg)
    {
        var name = cfg["Jwt:CookieName"];
        if (string.IsNullOrWhiteSpace(name)) name = "__Host-rt";
        var domain = cfg["Jwt:CookieDomain"];
        var secure = cfg.GetValue<bool?>("Jwt:CookieSecure") ?? true;
        var sameSiteStr = cfg["Jwt:CookieSameSite"] ?? "None";
        var sameSite = sameSiteStr.Equals("None", StringComparison.OrdinalIgnoreCase) ? SameSiteMode.None :
                         sameSiteStr.Equals("Strict", StringComparison.OrdinalIgnoreCase) ? SameSiteMode.Strict :
                         SameSiteMode.Lax;

        var opts = new CookieOptions
        {
            HttpOnly = true,
            Secure = secure,
            SameSite = sameSite,
            Expires = DateTimeOffset.UnixEpoch,
            Path = "/",
            IsEssential = true
        };

        if (!name.StartsWith("__Host-") && !string.IsNullOrWhiteSpace(domain))
            opts.Domain = domain;

        http.Response.Cookies.Append(name, "", opts);
    }
}

