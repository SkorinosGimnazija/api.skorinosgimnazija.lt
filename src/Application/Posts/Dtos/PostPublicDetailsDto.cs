namespace SkorinosGimnazija.Application.Posts.Dtos;

public record PostPublicDetailsDto : PostPublicDto
{
    public string? Meta { get; init; }

    public string? Text { get; init; }
}