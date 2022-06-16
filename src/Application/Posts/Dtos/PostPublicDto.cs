namespace SkorinosGimnazija.Application.Posts.Dtos;

public record PostPublicDto
{
    public int Id { get; init; }

    public DateTime PublishedAt { get; init; }

    public DateTime? ModifiedAt { get; init; }

    public string Slug { get; init; } = default!;

    public string Title { get; init; } = default!;

    public string? IntroText { get; init; }

    public string? FeaturedImage { get; init; }
}