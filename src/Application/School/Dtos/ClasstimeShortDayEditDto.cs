namespace SkorinosGimnazija.Application.School.Dtos;

public record ClasstimeShortDayEditDto : ClasstimeShortDayCreateDto
{
    public int Id { get; init; }
}