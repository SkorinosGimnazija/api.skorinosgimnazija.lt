namespace Application.Posts.Dtos;

using Categories.Dtos;
using System.ComponentModel.DataAnnotations;

public record PostDetailsDto : PostDto
{
    [Required]
    public List<string> Files { get; init; } = default!;

    [Required]
    public List<string> Images { get; init; } = default!;

    public string? Text { get; init; }
}