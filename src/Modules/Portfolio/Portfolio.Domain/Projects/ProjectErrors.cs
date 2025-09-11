using SharedKernel;

namespace Portfolio.Domain.Projects;

public static class ProjectErrors
{
    public static Error NotFound(Guid projectId) =>
        Error.NotFound("Projects.NotFound", $"The project with the Id = '{projectId}' was not found");

    public static Error NotFoundBySlug(string slug) =>
        Error.NotFound("Projects.NotFoundBySlug", $"The project with the Slug = '{slug}' was not found");

    public static readonly Error SlugNotUnique =
        Error.Conflict("Projects.SlugNotUnique", "The provided slug is not unique");

    public static readonly Error SlugInvalid =
        Error.Failure("Projects.SlugInvalid", "The slug must contain only lowercase letters, numbers, and hyphens");

    public static readonly Error NameRequired =
        Error.Failure("Projects.NameRequired", "The project name is required");

    public static readonly Error SummaryRequired =
        Error.Failure("Projects.SummaryRequired", "The project summary is required");

    public static readonly Error SourceInvalid =
        Error.Failure("Projects.SourceInvalid", "The project source is invalid");

    public static readonly Error Unauthorized =
        Error.Failure("Projects.Unauthorized", "You are not authorized to perform this action.");

    public static readonly Error Forbidden =
        Error.Failure("Projects.Forbidden", "You do not have permission to perform this action.");

    public static readonly Error ConcurrencyConflict =
        Error.Conflict("Projects.ConcurrencyConflict", "The resource was modified by another process.");

    public static Error TechnologyNotFound(Guid technologyId) =>
        Error.NotFound("Projects.TechnologyNotFound", $"The technology with Id = '{technologyId}' was not found");

    public static Error TechnologyNotFoundByName(string name) =>
        Error.NotFound("Projects.TechnologyNotFoundByName", $"The technology '{name}' was not found");

    public static readonly Error TechnologyNameNotUnique =
        Error.Conflict("Projects.TechnologyNameNotUnique", "The technology name must be unique");

    public static readonly Error DuplicateTechnologyInRequest =
        Error.Conflict("Projects.DuplicateTechnologyInRequest", "Duplicate technologies were provided in the request");

    public static Error TechnologyInUse(Guid technologyId) =>
        Error.Conflict("Projects.TechnologyInUse", $"The technology '{technologyId}' is referenced by one or more projects");

    public static Error TagNotFound(Guid tagId) =>
        Error.NotFound("Projects.TagNotFound", $"The tag with Id = '{tagId}' was not found");

    public static Error TagNotFoundByName(string name) =>
        Error.NotFound("Projects.TagNotFoundByName", $"The tag '{name}' was not found");

    public static readonly Error TagNameNotUnique =
        Error.Conflict("Projects.TagNameNotUnique", "The tag name must be unique");

    public static readonly Error DuplicateTagInRequest =
        Error.Conflict("Projects.DuplicateTagInRequest", "Duplicate tags were provided in the request");

    public static Error TagInUse(Guid tagId) =>
        Error.Conflict("Projects.TagInUse", $"The tag '{tagId}' is referenced by one or more projects");

    public static Error AssetNotFound(Guid assetId) =>
        Error.NotFound("Projects.AssetNotFound", $"The asset with Id = '{assetId}' was not found");

    public static readonly Error AssetUrlInvalid =
        Error.Failure("Projects.AssetUrlInvalid", "The asset URL is invalid");

    public static readonly Error AssetKindInvalid =
        Error.Failure("Projects.AssetKindInvalid", "The asset kind is invalid");

    public static readonly Error DuplicateAssetInRequest =
        Error.Conflict("Projects.DuplicateAssetInRequest", "Duplicate assets were provided in the request");

    public static readonly Error ShowcaseNotAllowedForExternal =
        Error.Conflict("Projects.ShowcaseNotAllowedForExternal", "Showcase can only be set when source is Aspire");

    public static readonly Error ShowcaseServiceNameRequired =
        Error.Failure("Projects.ShowcaseServiceNameRequired", "Showcase service name is required");

    public static Error ShowcaseNotFound(Guid projectId) =>
        Error.NotFound("Projects.ShowcaseNotFound", $"The showcase for project Id = '{projectId}' was not found");

    public static readonly Error ShowcaseAlreadyExists =
        Error.Conflict("Projects.ShowcaseAlreadyExists", "The project already has a showcase associated");

    public static readonly Error InvalidPayload =
        Error.Failure("Projects.InvalidPayload", "The request payload is invalid");
}
