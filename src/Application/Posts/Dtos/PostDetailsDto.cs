namespace SkorinosGimnazija.Application.Posts.Dtos;

public record PostDetailsDto : PostDto
{
    public List<string>? Files { get; init; } 

    public List<string>? Images { get; init; }

    public string? Text { get; init; }
}