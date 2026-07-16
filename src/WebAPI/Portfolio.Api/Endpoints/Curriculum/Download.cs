using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;

namespace Portfolio.Api.Endpoints.Curriculum;

internal sealed class Download : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/curriculum/{language}/file", async (
                [FromRoute] string language,
                [FromServices] IApplicationDbContext db,
                CancellationToken ct) =>
            {
                var normalized = NormalizeLanguage(language);
                var asset = await db.CurriculumAssets
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Language == normalized, ct);

                if (asset?.FileContent is not { Length: > 0 })
                {
                    return Results.NotFound();
                }

                return Results.File(
                    asset.FileContent,
                    asset.ContentType ?? "application/pdf",
                    enableRangeProcessing: true);
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
