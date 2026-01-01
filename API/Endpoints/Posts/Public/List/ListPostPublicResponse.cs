namespace API.Endpoints.Posts.Public.List;

using JetBrains.Annotations;

[PublicAPI]
public record ListPostPublicResponse
{
    public required int Id { get; init; }

    public required string Slug { get; init; }

    public required DateTime PublishedAt { get; init; }

    public DateTime? ModifiedAt { get; init; }

    public required string Title { get; init; }

    public string? FeaturedImage { get; init; }

    public string? IntroText { get; init; }
}