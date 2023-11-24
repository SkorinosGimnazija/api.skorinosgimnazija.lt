namespace SkorinosGimnazija.Application.Timetable.Dtos;

public record TimetableEditDto : TimetableCreateDto
{
    public int Id { get; init; }
}