namespace SkorinosGimnazija.Application.Posts.Dtos;

public record PostPatchDto
{
    public bool? IsFeatured { get; init; }

    public bool? IsPublished { get; init; }
}