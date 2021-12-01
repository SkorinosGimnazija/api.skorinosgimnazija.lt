namespace SkorinosGimnazija.Application.Posts.Dtos;

public record PostEditDto : PostCreateDto
{
    public int Id { get; init; }

    public ICollection<string>? Files { get; init; } 
     
    public ICollection<string>? Images { get; init; }
}