namespace API.Endpoints.Languages;

using JetBrains.Annotations;

[PublicAPI]
public record LanguageResponse
{
    public required string Id { get; init; }

    public required string Name { get; init; }
}