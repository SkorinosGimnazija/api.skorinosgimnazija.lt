namespace SkorinosGimnazija.Application.Timetable.Dtos;

public record TimetableSimpleDto
{
    public int Id { get; init; }

    public string ClassRoom { get; init; } = default!;

    public string ClassName { get; init; } = default!;
}