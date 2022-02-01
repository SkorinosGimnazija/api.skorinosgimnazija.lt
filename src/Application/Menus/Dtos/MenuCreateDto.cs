namespace SkorinosGimnazija.Application.Menus.Dtos;

public record MenuCreateDto
{
    public int Order { get; init; }

    public string Title { get; init; } = default!;

    public string? Url { get; init; }

    public bool IsPublished { get; init; }

    public string Slug { get; init; } = default!;

    public int LanguageId { get; init; }

    public int MenuLocationId { get; init; }

    public int? LinkedPostId { get; set; }

    public int? ParentMenuId { get; init; }
}