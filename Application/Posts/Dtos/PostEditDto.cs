namespace Application.Posts.Dtos;

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

public record PostEditDto : PostCreateDto
{
    [Required]
    public int Id { get; init; }

    public IEnumerable<string>? Files { get; init; }

    public IEnumerable<string>? Images { get; init; }
}