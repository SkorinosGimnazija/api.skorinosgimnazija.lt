namespace Application.Menus.Dtos;

using System.ComponentModel.DataAnnotations;

public record MenuLocationDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    public string Name { get; init; } = default!;

    [Required]
    public string Slug { get; init; } = default!;


    public List<MenuDto>? Menus { get; set; }
}