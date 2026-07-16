namespace Portfolio.Api.Contracts.Curriculum;

public sealed record UploadCurriculumAssetRequest(
    string Language,
    string FileName,
    string ContentType,
    string Base64Content);
