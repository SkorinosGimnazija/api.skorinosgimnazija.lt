namespace SkorinosGimnazija.Application.Posts.Dtos;

public record PostIndexDto
{
    // ReSharper disable once InconsistentNaming
    public string ObjectID { get; init; } = default!;

    public DateTime PublishedAt { get; init; }

    public  bool IsPublished { get; init; }

    public string Title { get; init; } = default!;

    public string? Text { get; init; }
}