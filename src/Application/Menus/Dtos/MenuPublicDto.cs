namespace SkorinosGimnazija.Application.Menus.Dtos;

public record MenuPublicDto
{
    public int Id { get; init; }

    public string? Url { get; init; }

    public string Title { get; init; } = default!;

    public string Slug { get; init; } = default!;

    public string Position { get; init; } = default!;

    public string Path { get; init; } = default!;

    public int? ParentMenuId { get; init; }

    public List<MenuPublicDto> ChildMenus { get; init; } = new();
}