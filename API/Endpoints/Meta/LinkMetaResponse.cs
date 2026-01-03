namespace API.Endpoints.Meta;

using JetBrains.Annotations;

[PublicAPI]
public record LinkMetaResponse
{
    public required string Ln { get; init; }

    public required string Url { get; init; }

    public required DateTime Date { get; init; }
}