namespace SkorinosGimnazija.Application.Banners.Dtos;

using Languages.Dtos;

public record BannerDto
{
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    public string Url { get; init; } = default!;

    public int Width { get; set; }

    public int Height { get; set; }

    public bool IsPublished { get; init; }

    public string PictureUrl { get; init; } = default!;

    public int Order { get; init; }

    public LanguageDto Language { get; init; } = default!;
}