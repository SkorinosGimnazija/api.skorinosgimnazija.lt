namespace Application.Posts.Dtos;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public record PostEditDto
{
    [Required]
    public int Id { get; init; }

    [Required]
    public bool IsFeatured { get; init; }

    public IEnumerable<string>? Files { get; init; }

    public IEnumerable<string>? Images { get; init; }

    public IFormFileCollection? NewFiles { get; init; }

    public IFormFileCollection? NewImages { get; init; }

    [Required]
    public DateTime PublishDate { get; init; }

    public DateTime? ModifiedDate { get; init; }

    public string? IntroText { get; init; }

    [Required]
    public bool IsPublished { get; init; }

    [Required]
    public int CategoryId { get; init; }

    [Required]
    public string Slug { get; init; }

    public string? Text { get; init; }

    [Required]
    public string Title { get; init; }
}