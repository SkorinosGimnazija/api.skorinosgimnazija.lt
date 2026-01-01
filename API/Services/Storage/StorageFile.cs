namespace API.Services.Storage;

public record StorageFile
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required string ContentType { get; init; }

    public required byte[] Data { get; init; }
}