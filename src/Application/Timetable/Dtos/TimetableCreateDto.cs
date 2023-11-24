namespace SkorinosGimnazija.Application.Timetable.Dtos;

public record TimetableCreateDto
{
    public int DayId { get; set; }

    public int TimeId { get; set; }

    public int RoomId { get; set; }

    public string ClassName { get; set; } = default!;
}