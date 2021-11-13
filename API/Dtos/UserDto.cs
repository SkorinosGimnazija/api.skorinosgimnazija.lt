namespace API.Dtos;

using System.ComponentModel.DataAnnotations;

public record UserDto
{
    [Required]
    public IEnumerable<string> Roles { get; init; } = default!;

    [Required]
    public string Email { get; init; } = default!;

    [Required]
    public string DisplayName { get; init; } = default!;
}