namespace SkorinosGimnazija.Application.School.Dtos;

public record ClassroomEditDto : ClassroomCreateDto
{
    public int Id { get; init; }
}