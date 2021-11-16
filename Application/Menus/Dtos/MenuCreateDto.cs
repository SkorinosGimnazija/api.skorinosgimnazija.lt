namespace Application.Menus.Dtos;

using System.ComponentModel.DataAnnotations;

public record MenuCreateDto
{
    public int Order { get; init; }

    [Required]
    public string Name { get; init; } = default!;

    public bool IsPublished { get; init; }

    public string? Slug { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int LanguageId { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int MenuLocationId { get; init; }

    public string? Url { get; init; }

    public int? ParentMenuId { get; init; }
}