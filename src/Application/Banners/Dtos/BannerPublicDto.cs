namespace SkorinosGimnazija.Application.Banners.Dtos;

public record BannerPublicDto
{
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    public string Url { get; init; } = default!;

    public int Width { get; set; }

    public int Height { get; set; }

    public string PictureUrl { get; init; } = default!;
}