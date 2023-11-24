namespace SkorinosGimnazija.Application.Timetable.Dtos;

using School.Dtos;

public record TimetablePublicDto
{
    public List<TimetableSimpleDto> Timetable { get; init; } = default!;

    public ClasstimeSimpleDto Classtime { get; init; } = default!;

    public string CurrentTime { get; init; } = default!;
}