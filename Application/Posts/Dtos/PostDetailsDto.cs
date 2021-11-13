namespace Application.Posts.Dtos;

using Categories.Dtos;
using System.ComponentModel.DataAnnotations;

public record PostDetailsDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    public int CategoryId { get; init; }

    [Required]
    public bool IsFeatured { get; init; }

    [Required]
    public List<string> Files { get; init; } = default!;

    [Required]
    public List<string> Images { get; init; } = default!;

    public string? IntroText { get; init; }

    [Required]
    public bool IsPublished { get; init; }

    [Required]
    public CategoryDto Category { get; init; } = default!;

    [Required]
    public DateTime PublishDate { get; init; }

    public DateTime? ModifiedDate { get; init; }

    [Required]
    public string Slug { get; init; } = default!;

    public string? Text { get; init; }

    [Required]
    public string Title { get; init; } = default!;
}