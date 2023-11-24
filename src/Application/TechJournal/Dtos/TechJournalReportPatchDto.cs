namespace SkorinosGimnazija.Application.TechJournal.Dtos;

public record TechJournalReportPatchDto
{
    public bool? IsFixed { get; init; }

    public string? Notes { get; init; }
}