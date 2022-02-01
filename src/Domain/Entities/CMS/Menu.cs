namespace SkorinosGimnazija.Domain.Entities;

public sealed class Menu
{
    public int Id { get; set; }

    public int Order { get; set; }

    public bool IsPublished { get; set; }

    public string Title { get; set; } = default!;

    public string Slug { get; set; } = default!;

    public string Path { get; set; } = default!;

    public string? Url { get; set; }

    public int? LinkedPostId { get; set; }

    public Post? LinkedPost { get; set; }

    public int LanguageId { get; set; }

    public Language Language { get; set; } = default!;

    public int? ParentMenuId { get; set; }

    public Menu? ParentMenu { get; set; }

    public int MenuLocationId { get; set; }

    public MenuLocation MenuLocation { get; set; } = default!;
}