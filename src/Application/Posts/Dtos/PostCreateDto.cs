namespace SkorinosGimnazija.Application.Posts.Dtos;

using Microsoft.AspNetCore.Http;

public record PostCreateDto
{
    public bool IsFeatured { get; init; }

    public IFormFileCollection? NewFiles { get; init; }

    public IFormFileCollection? NewImages { get; init; }

    public IFormFile? NewFeaturedImage { get; init; }

    public DateTime PublishedAt { get; init; }

    public DateTime? ModifiedAt { get; init; }

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