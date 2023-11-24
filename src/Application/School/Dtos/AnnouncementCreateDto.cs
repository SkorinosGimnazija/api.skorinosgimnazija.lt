namespace SkorinosGimnazija.Application.School.Dtos;

public record AnnouncementCreateDto
{
    public string Title { get; init; } = default!;

    public DateOnly StartTime { get; init; }

    public DateOnly EndTime { get; init; }
}