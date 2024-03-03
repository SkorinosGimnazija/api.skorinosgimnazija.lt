namespace SkorinosGimnazija.Application.BullyJournal.Dtos;

public record BullyJournalReportDetailsDto : BullyJournalReportDto
{
    public string Details { get; init; } = default!;

    public string Actions { get; init; } = default!;
}