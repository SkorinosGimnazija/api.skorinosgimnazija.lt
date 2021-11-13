namespace Application.Menus.Dtos;

using System.ComponentModel.DataAnnotations;

public record MenuCreateDto
{
    [Required]
    public int Order { get; init; }

    [Required]
    public string Name { get; init; } = default!;

    [Required]
    public bool IsPublished { get; init; }

    public string? Slug { get; init; }

    [Required]
    public int LanguageId { get; init; }

    public string? Url { get; init; }

    public int? ParentMenuId { get; init; }
}