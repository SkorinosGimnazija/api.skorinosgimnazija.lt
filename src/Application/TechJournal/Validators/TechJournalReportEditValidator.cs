namespace SkorinosGimnazija.Application.TechJournal.Mapping;
using FluentValidation;
using SkorinosGimnazija.Application.BullyJournal.Validators;

using SkorinosGimnazija.Application.BullyReports.Dtos;
using SkorinosGimnazija.Application.TechJournal.Dtos;
using SkorinosGimnazija.Application.TechJournal.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class TechJournalReportEditValidator : AbstractValidator<TechJournalReportEditDto>
{
    public TechJournalReportEditValidator()
    {
        Include(new TechJournalReportCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}