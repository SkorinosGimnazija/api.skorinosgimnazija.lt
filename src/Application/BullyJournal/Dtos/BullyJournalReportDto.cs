namespace SkorinosGimnazija.Application.BullyReports.Dtos;

public record BullyJournalReportDto
{
    public int Id { get; init; }
    
    public int UserId { get; init; } = default!;

    public string UserDisplayName { get; init; } = default!;

    public string BullyInfo { get; init; } = default!;

    public string VictimInfo { get; init; } = default!;
     
    public DateTime Date { get; init; }
}