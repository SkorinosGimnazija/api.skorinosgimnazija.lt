namespace API.Services.Revalidation;

using JetBrains.Annotations;

[PublicAPI]
public record RevalidationRequest
{
    public required string Tag { get; init; }

    public int? Id { get; init; }

    public string? Slug { get; init; }
}