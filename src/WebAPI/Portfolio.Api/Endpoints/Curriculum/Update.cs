using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Contracts.Curriculum;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Domain.Curriculum;

namespace Portfolio.Api.Endpoints.Curriculum;

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/curriculum", async (
                [FromBody] UpdateCurriculumAssetRequest request,
                [FromServices] IApplicationDbContext db,
                CancellationToken ct) =>
            {
                var normalized = NormalizeLanguage(request.Language);
                if (!Uri.TryCreate(request.Url, UriKind.Absolute, out var uri) ||
                    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                {
                    return Results.BadRequest(new { detail = "A valid absolute URL is required." });
                }

                var asset = await db.CurriculumAssets.FirstOrDefaultAsync(x => x.Language == normalized, ct);
                if (asset is null)
                {
                    asset = new CurriculumAsset
                    {
                        Id = Guid.NewGuid(),
                        Language = normalized,
                        Url = request.Url.Trim()
                    };
                    db.CurriculumAssets.Add(asset);
                }
                else
                {
                    asset.Url = request.Url.Trim();
                }

                await db.SaveChangesAsync(ct);
                return Results.NoContent();
            })
            .WithTags(Tags.Curriculum)
            .RequireAuthorization("SuperAdminOnly");
    }

    private static string NormalizeLanguage(string? language)
    {
        return string.Equals(language, "en-US", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(language, "us", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(language, "en", StringComparison.OrdinalIgnoreCase)
            ? "en-US"
            : "pt-BR";
    }
}
