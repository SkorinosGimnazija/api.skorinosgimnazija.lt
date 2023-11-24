namespace SkorinosGimnazija.Application.BullyJournal.Dtos;

public record BullyJournalReportEditDto : BullyJournalReportCreateDto
{
    public int Id { get; init; }
}