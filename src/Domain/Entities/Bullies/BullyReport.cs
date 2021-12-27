namespace SkorinosGimnazija.Domain.Entities.Bullies;

public class BullyReport
{
    public int Id { get; set; }

    public string BullyInfo { get; set; } = default!;

    public string VictimInfo { get; set; } = default!;

    public string? ReporterInfo { get; set; }

    public string? Details { get; set; }
     
    public string Location { get; set; } = default!;

    public DateTime Date { get; set; }

    public DateTime CreatedAt { get; set; }
}