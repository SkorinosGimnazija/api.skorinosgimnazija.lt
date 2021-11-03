namespace Application.Posts.Dtos;

using System.ComponentModel.DataAnnotations;
using Categories.Dtos;

public record PostDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    public bool IsFeatured { get; init; }

    [Required]
    public bool IsPublished { get; init; }

    [Required]
    public CategoryDto Category { get; init; }

    [Required]
    public DateTime PublishDate { get; init; }

    public DateTime? ModifiedDate { get; init; }

    [Required]
    public string Slug { get; init; }

    [Required]
    public string Title { get; init; }
}