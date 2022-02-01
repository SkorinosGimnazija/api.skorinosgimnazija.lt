namespace SkorinosGimnazija.Application.Menus.Dtos;

public record MenuDto
{
    public int Id { get; init; }

    public int Order { get; init; }

    public string? Url { get; init; }

    public string Title { get; init; } = default!;

    public string Slug { get; init; } = default!;

    public string Path { get; init; } = default!;

    public bool IsPublished { get; init; }

    public int? ParentMenuId { get; init; }

    public List<MenuDto> ChildMenus { get; init; } = new();
}