namespace Application.Posts.Dtos;

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public record PostCreateDto
{
    [Required]
    public bool IsFeatured { get; init; }

    public IFormFileCollection? NewFiles { get; init; }

    public IFormFileCollection? NewImages { get; init; }

    [Required]
    public DateTime PublishDate { get; init; }

    public DateTime? ModifiedDate { get; init; }

    public string? IntroText { get; init; }

    [Required]
    public bool IsPublished { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int CategoryId { get; init; }

    [Required]
    public string Slug { get; init; } = default!;

    public string? Text { get; init; }

    [Required]
    public string Title { get; init; } = default!;
}