namespace SkorinosGimnazija.Application.BullyReports.Dtos;

public record BullyJournalReportEditDto : BullyJournalReportCreateDto
{
    public int Id { get; init; }
}