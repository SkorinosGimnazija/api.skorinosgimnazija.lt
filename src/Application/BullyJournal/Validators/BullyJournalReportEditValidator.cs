namespace SkorinosGimnazija.Application.BullyJournal.Validators;

using Dtos;
using FluentValidation;

internal class BullyJournalReportEditValidator : AbstractValidator<BullyJournalReportEditDto>
{
    public BullyJournalReportEditValidator()
    {
        Include(new BullyJournalReportCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}