namespace SkorinosGimnazija.Application.School.Dtos;

public record ClasstimeSimpleDto
{
    public int Number { get; init; }

    public string StartTime { get; init; } = default!;

    public string EndTime { get; init; } = default!;
}