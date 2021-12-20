namespace SkorinosGimnazija.Domain.Entities;

public sealed class Post
{
    public int Id { get; set; }

    public bool IsFeatured { get; set; }

    public bool IsPublished { get; set; }

    public bool ShowInFeed { get; set; }

    public DateTime PublishDate { get; set; }
     
    public int LanguageId { get; set; }

    public Language Language { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Slug { get; set; } = default!;

    public string? IntroText { get; set; }

    public string? Text { get; set; }

    public string? Meta { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public List<string>? Files { get; set; }

    public List<string>? Images { get; set; }
}