namespace SkorinosGimnazija.Application.Timetable.Dtos;

public record TimetableDeleteDayDto
{
    public List<int> DayIds { get; set; } = new();
}