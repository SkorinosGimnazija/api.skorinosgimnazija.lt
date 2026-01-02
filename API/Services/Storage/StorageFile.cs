namespace API.Services.Storage;

public record StorageFile : FileMetadata
{
    public required byte[] Data { get; init; }
}

public record FileMetadata
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required string ContentType { get; init; }
}