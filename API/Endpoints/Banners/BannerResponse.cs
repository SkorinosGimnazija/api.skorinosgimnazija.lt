namespace API.Endpoints.Banners;

using JetBrains.Annotations;

[PublicAPI]
public record BannerResponse
{
    public required int Id { get; init; }

    public required string Title { get; init; }

    public required string Url { get; init; }

    public required int Width { get; init; }

    public required int Height { get; init; }

    public required bool IsPublished { get; init; }

    public required string ImageUrl { get; init; }

    public required int Order { get; init; }

    public required string LanguageId { get; init; }

    public required string Language { get; set; }
}