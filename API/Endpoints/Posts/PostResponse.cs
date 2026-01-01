namespace API.Endpoints.Posts;

using JetBrains.Annotations;

[PublicAPI]
public record PostResponse
{
    public required int Id { get; init; }

    public required bool IsFeatured { get; init; }

    public required bool IsPublished { get; init; }

    public required bool ShowInFeed { get; init; }

    public required DateTime PublishedAt { get; init; }

    public required string LanguageId { get; init; }

    public required string Language { get; init; }

    public required string Title { get; init; }

    public required string Slug { get; init; }

    public string? IntroText { get; init; }

    public string? Text { get; init; }

    public string? FeaturedImage { get; init; }

    public string? Meta { get; init; }

    public DateTime? ModifiedAt { get; init; }

    public List<string>? Files { get; init; }

    public List<string>? Images { get; init; }
}