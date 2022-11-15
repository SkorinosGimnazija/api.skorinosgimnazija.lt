namespace SkorinosGimnazija.Application.TechJournal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record TechJournalReportEditDto : TechJournalReportCreateDto
{
    public int Id { get; init; }
}
