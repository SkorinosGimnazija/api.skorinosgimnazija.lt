namespace SkorinosGimnazija.Domain.Entities.TechReports;

using Identity;

public class TechJournalReport
{
    public int Id { get; set; }

    public AppUser User { get; set; } = default!;

    public int UserId { get; set; }

    public string Place { get; set; } = default!;

    public string Details { get; set; } = default!;

    public string? Notes { get; set; }

    public bool? IsFixed { get; set; }

    public DateTime ReportDate { get; set; }

    public DateTime? FixDate { get; set; }
}