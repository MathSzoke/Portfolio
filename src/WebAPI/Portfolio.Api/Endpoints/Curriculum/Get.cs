using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Contracts.Curriculum;
using Portfolio.Application.Abstractions.Data;

namespace Portfolio.Api.Endpoints.Curriculum;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/curriculum", async (
                [FromQuery] string language,
                HttpRequest request,
                [FromServices] IApplicationDbContext db,
                CancellationToken ct) =>
            {
                var normalized = NormalizeLanguage(language);
                var asset = await db.CurriculumAssets
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Language == normalized, ct);

                var url = asset is { FileContent.Length: > 0 }
                    ? $"{request.Scheme}://{request.Host}/api/v1/curriculum/{normalized}/file?v={asset.UpdatedAt.Ticks}"
                    : NormalizeStoredUrl(asset?.Url);

                return Results.Ok(new CurriculumAssetResponse(normalized, url ?? string.Empty));
            })
            .WithTags(Tags.Curriculum);
    }

    private static string? NormalizeStoredUrl(string? url)
    {
        return url is not null &&
               url.Contains("res.cloudinary.com", StringComparison.OrdinalIgnoreCase) &&
               url.Contains("/raw/upload/", StringComparison.OrdinalIgnoreCase)
            ? null
            : url;
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
