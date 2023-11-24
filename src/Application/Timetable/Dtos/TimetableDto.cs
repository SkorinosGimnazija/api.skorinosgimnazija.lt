namespace SkorinosGimnazija.Application.Timetable.Dtos;

using School.Dtos;

public record TimetableDto
{
    public int Id { get; init; }

    public ClassdayDto Day { get; init; } = default!;

    public ClassroomDto Room { get; init; } = default!;

    public ClasstimeDto Time { get; init; } = default!;

    public string ClassName { get; init; } = default!;
}