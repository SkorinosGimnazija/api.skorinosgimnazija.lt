namespace API.Endpoints.Posts.Public.Get;

using JetBrains.Annotations;

[PublicAPI]
public record GetPostPublicResponse
{
    public required int Id { get; init; }

    public required string Slug { get; init; }

    public required DateTime PublishedAt { get; init; }

    public DateTime? ModifiedAt { get; init; }

    public required string Title { get; init; }

    public string? Text { get; init; }

    public string? Meta { get; init; }

    public List<string>? Images { get; init; }
}