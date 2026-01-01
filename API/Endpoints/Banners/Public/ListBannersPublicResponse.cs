namespace API.Endpoints.Banners.Public;

using JetBrains.Annotations;

[PublicAPI]
public record ListBannersPublicResponse
{
    public required int Id { get; init; }

    public required string Title { get; init; }

    public required string Url { get; init; }

    public required string ImageUrl { get; init; }

    public required int Width { get; init; }

    public required int Height { get; init; }
}