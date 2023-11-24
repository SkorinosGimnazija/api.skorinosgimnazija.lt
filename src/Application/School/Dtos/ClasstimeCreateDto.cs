namespace SkorinosGimnazija.Application.School.Dtos;

public record ClasstimeCreateDto
{
    public int Number { get; init; }

    public TimeOnly StartTime { get; init; }

    public TimeOnly StartTimeShort { get; init; }

    public TimeOnly EndTime { get; init; }

    public TimeOnly EndTimeShort { get; init; }
}