using System.Text.Json;
using Microsoft.SemanticKernel.ChatCompletion;
using Portfolio.Api.Contracts.Experiences;
using Portfolio.Domain.Experiences;

namespace Portfolio.Api.Endpoints.Experiences;

internal static class ExperienceMapper
{
    public static ExperienceItemResponse ToResponse(this ExperienceItem item, string language)
    {
        var useEnglish = NormalizeLanguage(language) == "en-US";
        return new ExperienceItemResponse(
            item.Id,
            item.Company,
            item.LogoUrl,
            useEnglish ? item.RoleEnUs : item.RolePtBr,
            useEnglish ? item.PeriodEnUs : item.PeriodPtBr,
            useEnglish ? item.LocationEnUs : item.LocationPtBr,
            useEnglish ? item.DescriptionEnUs : item.DescriptionPtBr,
            item.Techs,
            item.StartDate,
            item.EndDate,
            item.IsPresent,
            item.SortOrder,
            new ExperienceItemTranslations(
                new ExperienceItemTranslation(item.RolePtBr, item.PeriodPtBr, item.LocationPtBr, item.DescriptionPtBr),
                new ExperienceItemTranslation(item.RoleEnUs, item.PeriodEnUs, item.LocationEnUs, item.DescriptionEnUs)));
    }

    public static async Task ApplyRequestAsync(
        this ExperienceItem item,
        ExperienceItemRequest request,
        IServiceProvider services,
        CancellationToken ct)
    {
        var source = NormalizeLanguage(request.SourceLanguage);
        var normalized = new ExperienceItemTranslation(
            request.Role.Trim(),
            request.Period.Trim(),
            request.Location.Trim(),
            CleanArray(request.Description));

        var translated = await TranslateAsync(normalized, source, services, ct);

        item.Company = request.Company.Trim();
        item.LogoUrl = request.LogoUrl.Trim();
        item.Techs = CleanArray(request.Techs);
        item.StartDate = string.IsNullOrWhiteSpace(request.StartDate) ? null : request.StartDate.Trim();
        item.EndDate = string.IsNullOrWhiteSpace(request.EndDate) ? null : request.EndDate.Trim();
        item.IsPresent = request.IsPresent;
        item.SortOrder = request.SortOrder;

        if (source == "en-US")
        {
            item.RoleEnUs = normalized.Role;
            item.PeriodEnUs = normalized.Period;
            item.LocationEnUs = normalized.Location;
            item.DescriptionEnUs = normalized.Description;
            item.RolePtBr = translated.Role;
            item.PeriodPtBr = translated.Period;
            item.LocationPtBr = translated.Location;
            item.DescriptionPtBr = translated.Description;
        }
        else
        {
            item.RolePtBr = normalized.Role;
            item.PeriodPtBr = normalized.Period;
            item.LocationPtBr = normalized.Location;
            item.DescriptionPtBr = normalized.Description;
            item.RoleEnUs = translated.Role;
            item.PeriodEnUs = translated.Period;
            item.LocationEnUs = translated.Location;
            item.DescriptionEnUs = translated.Description;
        }
    }

    private static async Task<ExperienceItemTranslation> TranslateAsync(
        ExperienceItemTranslation source,
        string sourceLanguage,
        IServiceProvider services,
        CancellationToken ct)
    {
        var targetLanguage = sourceLanguage == "en-US" ? "pt-BR" : "en-US";
        var chat = services.GetService<IChatCompletionService>();
        if (chat is null)
        {
            return source;
        }

        try
        {
            var history = new ChatHistory();
            history.AddSystemMessage("You translate portfolio job experience content. Return only valid compact JSON with keys role, period, location, description. Preserve company names, dates, technologies and acronyms. Do not add markdown.");
            history.AddUserMessage(JsonSerializer.Serialize(new
            {
                sourceLanguage,
                targetLanguage,
                role = source.Role,
                period = source.Period,
                location = source.Location,
                description = source.Description
            }));

            var result = await chat.GetChatMessageContentAsync(history, cancellationToken: ct);
            var translated = JsonSerializer.Deserialize<ExperienceTranslationPayload>(
                result.Content ?? string.Empty,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return translated is null
                ? source
                : new ExperienceItemTranslation(
                    translated.Role ?? source.Role,
                    translated.Period ?? source.Period,
                    translated.Location ?? source.Location,
                    CleanArray(translated.Description ?? source.Description));
        }
        catch
        {
            return source;
        }
    }

    private static string[] CleanArray(IEnumerable<string>? values)
    {
        return values?
            .Select(x => x?.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Cast<string>()
            .ToArray() ?? [];
    }

    public static string NormalizeLanguage(string? language)
    {
        return string.Equals(language, "en-US", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(language, "us", StringComparison.OrdinalIgnoreCase) ||
               string.Equals(language, "en", StringComparison.OrdinalIgnoreCase)
            ? "en-US"
            : "pt-BR";
    }

    private sealed record ExperienceTranslationPayload(
        string? Role,
        string? Period,
        string? Location,
        string[]? Description);
}
