namespace SkorinosGimnazija.Application.BullyJournal.Dtos;

using SkorinosGimnazija.Application.BullyReports.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record BullyJournalReportDetailsDto : BullyJournalReportDto
{
    public string Details { get; init; } = default!;

    public string Actions { get; init; } = default!;
}
