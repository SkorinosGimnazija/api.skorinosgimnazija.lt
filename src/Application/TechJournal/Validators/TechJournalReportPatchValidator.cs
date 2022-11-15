namespace SkorinosGimnazija.Application.TechJournal.Validators;
using FluentValidation;

using SkorinosGimnazija.Application.TechJournal.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class TechJournalReportPatchValidator : AbstractValidator<TechJournalReportPatchDto>
{
    public TechJournalReportPatchValidator()
    {
        RuleFor(x => x.Notes).MaximumLength(512);
    }
}