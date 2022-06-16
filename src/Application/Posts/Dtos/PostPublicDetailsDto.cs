namespace SkorinosGimnazija.Application.Posts.Dtos;

public record PostPublicDetailsDto : PostPublicDto
{
    public List<string>? Images { get; init; }

    public string? Meta { get; init; }

    public string? Text { get; init; }
}