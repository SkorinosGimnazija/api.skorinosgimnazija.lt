namespace SkorinosGimnazija.Application.Posts.Dtos;

public record PostDetailsDto : PostDto
{
    public List<string> Files { get; init; } = default!;

    public List<string> Images { get; init; } = default!;

    public string? Text { get; init; }
}