namespace SkorinosGimnazija.Application.School.Dtos;

public record ClasstimeEditDto : ClasstimeCreateDto
{
    public int Id { get; init; }
}