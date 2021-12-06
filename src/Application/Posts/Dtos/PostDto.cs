namespace SkorinosGimnazija.Application.Posts.Dtos;

using Languages.Dtos;

public record PostDto
{
    public int Id { get; init; }

    public bool IsFeatured { get; init; }

    public bool IsPublished { get; init; }

    public bool ShowInFeed { get; init; }

    public DateTime PublishDate { get; init; }

    public DateTime? ModifiedDate { get; init; }

    public LanguageDto Language { get; init; } = default!;

    public string Slug { get; init; } = default!;

    public string Title { get; init; } = default!;

    public string? IntroText { get; init; }
    public string? Meta { get; init; }
}