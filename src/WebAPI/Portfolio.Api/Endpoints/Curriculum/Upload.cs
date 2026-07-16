using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Contracts.Curriculum;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Domain.Curriculum;

namespace Portfolio.Api.Endpoints.Curriculum;

internal sealed class Upload : IEndpoint
{
    private const int MaxPdfBytes = 10 * 1024 * 1024;

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/curriculum/upload", async (
                [FromBody] UploadCurriculumAssetRequest request,
                [FromServices] IApplicationDbContext db,
                CancellationToken ct) =>
            {
                var normalized = NormalizeLanguage(request.Language);
                var contentType = string.IsNullOrWhiteSpace(request.ContentType)
                    ? "application/pdf"
                    : request.ContentType.Trim();

                if (!string.Equals(contentType, "application/pdf", StringComparison.OrdinalIgnoreCase))
                {
                    return Results.BadRequest(new { detail = "Only PDF files are supported." });
                }

                byte[] fileContent;
                try
                {
                    fileContent = Convert.FromBase64String(CleanBase64(request.Base64Content));
                }
                catch (FormatException)
                {
                    return Results.BadRequest(new { detail = "Invalid PDF content." });
                }

                if (fileContent.Length == 0 || fileContent.Length > MaxPdfBytes || !IsPdf(fileContent))
                {
                    return Results.BadRequest(new { detail = "Invalid PDF file." });
                }

                var asset = await db.CurriculumAssets.FirstOrDefaultAsync(x => x.Language == normalized, ct);
                if (asset is null)
                {
                    asset = new CurriculumAsset
                    {
                        Id = Guid.NewGuid(),
                        Language = normalized
                    };
                    db.CurriculumAssets.Add(asset);
                }

                asset.Url = null;
                asset.FileContent = fileContent;
                asset.FileName = BuildSafeFileName(request.FileName, normalized);
                asset.ContentType = "application/pdf";

                await db.SaveChangesAsync(ct);
                return Results.NoContent();
            })
            .WithTags(Tags.Curriculum)
            .RequireAuthorization("SuperAdminOnly");
    }

    private static string CleanBase64(string value)
    {
        var markerIndex = value.IndexOf("base64,", StringComparison.OrdinalIgnoreCase);
        return markerIndex >= 0 ? value[(markerIndex + "base64,".Length)..] : value;
    }

    private static bool IsPdf(byte[] content)
    {
        return content.Length >= 4 &&
               content[0] == '%' &&
               content[1] == 'P' &&
               content[2] == 'D' &&
               content[3] == 'F';
    }

    private static string BuildSafeFileName(string fileName, string language)
    {
        var value = Path.GetFileName(fileName);
        if (string.IsNullOrWhiteSpace(value))
        {
            value = $"curriculum-{language}.pdf";
        }

        return value.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) ? value : $"{value}.pdf";
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
