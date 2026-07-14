using System.Net.Http.Headers;
using System.Text.Json;
using Google.Apis.Auth;
using Portfolio.Application.Abstractions.Security;

namespace Portfolio.Infrastructure.Authentication;

public sealed class ExternalAuthService(IHttpClientFactory httpClientFactory) : IGoogleAuthService, ILinkedInAuthService
{
    public async Task<GoogleUserInfo> ValidateIdToken(string idToken, string expectedClientId, CancellationToken ct)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings { Audience = [expectedClientId] };
        var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        var name = string.IsNullOrWhiteSpace(payload.Name) ? $"{payload.GivenName} {payload.FamilyName}".Trim() : payload.Name;
        return new GoogleUserInfo(payload.Subject, payload.Email, name, payload.Picture);
    }

    public async Task<string> ExchangeCodeForAccessToken(string code, string clientId, string clientSecret, string redirectUri, CancellationToken ct)
    {
        var http = httpClientFactory.CreateClient("linkedin");
        using var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = code,
            ["redirect_uri"] = redirectUri,
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret
        });
        using var res = await http.PostAsync("https://www.linkedin.com/oauth/v2/accessToken", form, ct);
        string content = await res.Content.ReadAsStringAsync(ct);
        if (!res.IsSuccessStatusCode)
            throw new Exception($"LinkedIn token error: {res.StatusCode} - {content} | redirect_uri: {redirectUri} | code: {code}");
        using var doc = JsonDocument.Parse(content);
        return doc.RootElement.GetProperty("access_token").GetString()!;
    }

    public async Task<LinkedInUserInfo> GetUserInfo(string accessToken, CancellationToken ct)
    {
        var http = httpClientFactory.CreateClient("linkedin");
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var userinfoRes = await http.GetAsync("https://api.linkedin.com/v2/userinfo", ct);
        userinfoRes.EnsureSuccessStatusCode();
        using var userinfoDoc = JsonDocument.Parse(await userinfoRes.Content.ReadAsStringAsync(ct));

        var id = userinfoDoc.RootElement.GetProperty("sub").GetString()!;
        var name = userinfoDoc.RootElement.TryGetProperty("name", out var nameProp) ? nameProp.GetString() ?? "" : "";
        var email = userinfoDoc.RootElement.TryGetProperty("email", out var emailProp) ? emailProp.GetString() ?? "" : "";
        var picture = userinfoDoc.RootElement.TryGetProperty("picture", out var picProp) ? picProp.GetString() ?? "" : null;

        return new LinkedInUserInfo(id, email, name, picture);
    }

}
