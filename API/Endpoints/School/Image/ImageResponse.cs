namespace API.Endpoints.School.Image;

using JetBrains.Annotations;

[PublicAPI]
public record ImageResponse
{
    public required string Url { get; init; }
}