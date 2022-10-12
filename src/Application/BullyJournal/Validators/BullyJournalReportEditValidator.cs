namespace SkorinosGimnazija.Application.Accomplishments.Validators;

using Dtos;
using FluentValidation;
using SkorinosGimnazija.Application.BullyJournal.Validators;
using SkorinosGimnazija.Application.BullyReports.Dtos;

internal class BullyJournalReportEditValidator : AbstractValidator<BullyJournalReportEditDto>
{
    public BullyJournalReportEditValidator()
    {
        Include(new BullyJournalReportCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}