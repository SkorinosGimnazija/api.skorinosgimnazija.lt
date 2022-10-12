namespace SkorinosGimnazija.Application.BullyReports.Dtos;

public record BullyJournalReportCreateDto
{
    public string BullyInfo { get; init; } = default!;

    public string VictimInfo { get; init; } = default!;

    public string Details { get; init; } = default!;

    public string Actions { get; init; } = default!;

    public DateTime Date { get; init; }
}