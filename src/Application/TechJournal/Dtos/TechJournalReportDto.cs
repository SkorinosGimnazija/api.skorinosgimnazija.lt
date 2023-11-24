namespace SkorinosGimnazija.Application.TechJournal.Dtos;

public record TechJournalReportDto
{
    public int Id { get; init; }

    public int UserId { get; init; } = default!;

    public string UserDisplayName { get; init; } = default!;

    public bool? IsFixed { get; set; }

    public string Place { get; init; } = default!;

    public DateTime? FixDate { get; init; }

    public DateTime ReportDate { get; init; }

    public string? Notes { get; init; }

    public string Details { get; init; } = default!;
}