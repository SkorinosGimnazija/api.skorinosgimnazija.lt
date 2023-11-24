namespace SkorinosGimnazija.Application.School.Dtos;

public record AnnouncementDto
{
    public int Id { get; init; }

    public string Title { get; init; } = default!;

    public DateOnly StartTime { get; init; }

    public DateOnly EndTime { get; init; }
}