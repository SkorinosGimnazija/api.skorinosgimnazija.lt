namespace API.Services.Storage;

public record UploadStreamMetadata
{
    public required string FileExtension { get; init; }

    public required string MimeType { get; init; }
}