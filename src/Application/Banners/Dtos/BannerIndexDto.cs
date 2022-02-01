namespace SkorinosGimnazija.Application.Banners.Dtos;

public record BannerIndexDto
{
    // ReSharper disable once InconsistentNaming
    public string ObjectID { get; init; } = default!;

    public string Title { get; init; } = default!;

    public string Url { get; init; } = default!;
}