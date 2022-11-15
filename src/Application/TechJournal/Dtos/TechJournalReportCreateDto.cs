namespace SkorinosGimnazija.Application.TechJournal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record TechJournalReportCreateDto
{
    public string Place { get; init; } = default!;

    public string Details { get; init; } = default!;
}
