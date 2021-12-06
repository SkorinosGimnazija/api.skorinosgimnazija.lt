namespace SkorinosGimnazija.Application.Posts.Dtos;

using Microsoft.AspNetCore.Http;

public record PostCreateDto
{
    public bool IsFeatured { get; init; }

    public IFormFileCollection? NewFiles { get; init; }

    public IFormFileCollection? NewImages { get; init; }

    public DateTime PublishDate { get; init; }

    public DateTime? ModifiedDate { get; init; }

    public string? IntroText { get; init; }

    public bool IsPublished { get; init; }

    public bool ShowInFeed { get; init; }

    public bool OptimizeImages { get; init; }

    public int LanguageId { get; init; }

    public string Slug { get; init; } = default!;
     
    public string? Text { get; init; }
    public string? Meta { get; init; }

    public string Title { get; init; } = default!;
}