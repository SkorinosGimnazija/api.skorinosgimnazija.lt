namespace SkorinosGimnazija.Application.School.Dtos;

public record ClasstimeShortDayDto
{
    public int Id { get; init; }

    public DateOnly Date { get; init; }
}