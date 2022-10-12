namespace SkorinosGimnazija.Application.BullyJournal.Validators;
using FluentValidation;

using SkorinosGimnazija.Application.Accomplishments.Dtos;
using SkorinosGimnazija.Application.BullyReports.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class BullyJournalReportCreateValidator : AbstractValidator<BullyJournalReportCreateDto>
{
    public BullyJournalReportCreateValidator()
    {
        RuleFor(x => x.BullyInfo).NotEmpty().MaximumLength(256);
        RuleFor(x => x.VictimInfo).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Details).NotEmpty().MaximumLength(2048);
        RuleFor(x => x.Actions).NotEmpty().MaximumLength(2048);
        RuleFor(x => x.Date).NotEmpty();
    }
}