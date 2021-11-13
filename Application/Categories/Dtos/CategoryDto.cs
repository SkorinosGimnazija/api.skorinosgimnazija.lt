namespace Application.Categories.Dtos;

using Languages.Dtos;
using System.ComponentModel.DataAnnotations;

public record CategoryDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    public string Name { get; init; } = default!;

    [Required]
    public string Slug { get; init; } = default!;

    [Required]
    public LanguageDto Language { get; init; } = default!;

    [Required]
    public bool ShowOnHomePage { get; init; }
}