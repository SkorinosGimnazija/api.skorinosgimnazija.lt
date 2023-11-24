namespace SkorinosGimnazija.Domain.Entities.School;

public class Announcement
{
    public int Id { get; set; }

    public string Title { get; set; } = default!;

    public DateOnly StartTime { get; set; }

    public DateOnly EndTime { get; set; }
}