namespace SkorinosGimnazija.Domain.Entities;

public sealed class Banner
{
    public int Id { get; set; }

    public string Title { get; set; } = default!;

    public string Url { get; set; } = default!;

    public bool IsPublished { get; set; }

    public string PictureUrl { get; set; } = default!;

    public int Order { get; set; }

    public int LanguageId { get; set; }

    public Language Language { get; set; } = default!;
}