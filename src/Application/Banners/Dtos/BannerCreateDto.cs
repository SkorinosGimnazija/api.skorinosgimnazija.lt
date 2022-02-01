namespace SkorinosGimnazija.Application.Banners.Dtos;

using Microsoft.AspNetCore.Http;

public record BannerCreateDto
{
    public string Title { get; init; } = default!;

    public string Url { get; init; } = default!;

    public bool IsPublished { get; init; }

    public int Width { get; set; }

    public int Height { get; set; }

    public IFormFile Picture { get; init; } = default!;

    public int Order { get; init; }

    public int LanguageId { get; init; }
}