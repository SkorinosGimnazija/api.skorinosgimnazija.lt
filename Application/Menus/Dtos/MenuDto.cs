namespace Application.Menus.Dtos;

using Languages.Dtos;
using System.ComponentModel.DataAnnotations;

public record MenuDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    public int Order { get; init; }

    [Required]
    public string Name { get; init; } = default!;

    public string? Slug { get; init; }

    [Required]
    public bool IsPublished { get; init; }

    [Required]
    public LanguageDto Language { get; init; } = default!;

    public string? Url { get; init; }

    public int? ParentMenuId { get; init; }
}