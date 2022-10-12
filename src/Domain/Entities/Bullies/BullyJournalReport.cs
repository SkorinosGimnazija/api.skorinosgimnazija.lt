namespace SkorinosGimnazija.Domain.Entities.Bullies;

using Identity;

public class BullyJournalReport
{
    public int Id { get; set; }

    public AppUser User { get; set; } = default!;

    public int UserId { get; set; }

    public string BullyInfo { get; set; } = default!;

    public string VictimInfo { get; set; } = default!;

    public string Details { get; set; } = default!;

    public string Actions { get; set; } = default!;

    public DateOnly Date { get; set; }
}