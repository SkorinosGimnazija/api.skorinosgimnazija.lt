namespace SkorinosGimnazija.Application.TechJournal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record  TechJournalReportDto
{
    public int Id { get; init; }

    public int UserId { get; init; } = default!;

    public string UserDisplayName { get; init; } = default!;

    public bool? IsFixed { get; set; }

    public string Place { get; init; } = default!;

    public DateTime? FixDate { get; init; }

    public DateTime ReportDate { get; init; }

    public string? Notes { get; init; }

    public string Details { get; init; } = default!;
}
