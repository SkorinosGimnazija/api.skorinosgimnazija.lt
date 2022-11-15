namespace SkorinosGimnazija.Application.TechJournal.Validators;
using FluentValidation;

using SkorinosGimnazija.Application.BullyReports.Dtos;
using SkorinosGimnazija.Application.TechJournal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class TechJournalReportCreateValidator : AbstractValidator<TechJournalReportCreateDto>
{
    public TechJournalReportCreateValidator()
    {
        RuleFor(x => x.Details).NotEmpty().MaximumLength(512);
        RuleFor(x => x.Place).NotEmpty().MaximumLength(64);
    }
}