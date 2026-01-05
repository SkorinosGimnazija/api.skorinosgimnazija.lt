namespace API.Endpoints.School.Image;

using JetBrains.Annotations;

[PublicAPI]
public record PostRandomImageResponse
{
    public required string Url { get; init; }

    public required int PostId { get; init; }

    public required string PostTitle { get; init; }

    public required DateTime PostDate { get; init; }
}