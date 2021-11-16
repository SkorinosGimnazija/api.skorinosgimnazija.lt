namespace Application.Categories.Dtos;

using System.ComponentModel.DataAnnotations;

public record CategoryEditDto : CategoryCreateDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int Id { get; init; }

 
}