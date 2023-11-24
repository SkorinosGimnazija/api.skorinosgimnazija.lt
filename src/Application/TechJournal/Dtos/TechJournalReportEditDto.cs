namespace SkorinosGimnazija.Application.TechJournal.Dtos;

public record TechJournalReportEditDto : TechJournalReportCreateDto
{
    public int Id { get; init; }
}