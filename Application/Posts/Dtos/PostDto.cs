namespace Application.Posts.Dtos;

using Categories.Dtos;
using System.ComponentModel.DataAnnotations;

public record PostDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    public bool IsFeatured { get; init; }

    [Required]
    public bool IsPublished { get; init; }

    [Required]
    public CategoryDto Category { get; init; } = default!;

    [Required]
    public DateTime PublishDate { get; init; }

    public DateTime? ModifiedDate { get; init; }

    [Required]
    public string Slug { get; init; } = default!;

    [Required]
    public string Title { get; init; } = default!;

    public string? Intro { get; init; }


}