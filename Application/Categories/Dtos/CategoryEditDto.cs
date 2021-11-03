namespace Application.Categories.Dtos;

using System.ComponentModel.DataAnnotations;

public record CategoryEditDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    public int LanguageId { get; init; }

    [Required]
    public string Name { get; init; }

    [Required]
    public string Slug { get; init; }

    [Required]
    public bool ShowOnHomePage { get; init; }
}