namespace SkorinosGimnazija.Application.TechJournal.Dtos;

public record TechJournalReportCreateDto
{
    public string Place { get; init; } = default!;

    public string Details { get; init; } = default!;
}