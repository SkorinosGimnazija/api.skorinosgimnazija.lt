namespace Application.Menus.Dtos;

using System.ComponentModel.DataAnnotations;

public record MenuEditDto : MenuCreateDto
{
    [Required]
    public int Id { get; init; }
}