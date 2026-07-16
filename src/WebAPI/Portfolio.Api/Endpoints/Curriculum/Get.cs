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
                [FromServices] IApplicationDbContext db,
                CancellationToken ct) =>
            {
                var normalized = NormalizeLanguage(language);
                var asset = await db.CurriculumAssets
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Language == normalized, ct);

                return asset is null
                    ? Results.NotFound()
                    : Results.Ok(new CurriculumAssetResponse(asset.Language, asset.Url));
            })
            .WithTags(Tags.Curriculum);
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
