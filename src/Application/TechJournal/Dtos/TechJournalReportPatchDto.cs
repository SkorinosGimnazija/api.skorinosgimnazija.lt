namespace SkorinosGimnazija.Application.TechJournal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record TechJournalReportPatchDto
{
    public bool? IsFixed { get; init; }

    public string? Notes { get; init; }
}
