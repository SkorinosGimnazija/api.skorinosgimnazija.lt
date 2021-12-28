namespace SkorinosGimnazija.Application.BullyReports.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record BullyReportDto
{
    public int Id { get; init; }

    public string BullyInfo { get; init; } = default!;

    public string VictimInfo { get; init; } = default!;

    public string? ReporterInfo { get; init; }

    public string? Details { get; init; }

    public string Location { get; init; } = default!;
    
    public DateTime Date { get; init; }

    public DateTime CreatedAt { get; init; }
}
