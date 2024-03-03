namespace SkorinosGimnazija.Application.TechJournal.Validators;

using Dtos;
using FluentValidation;

internal class TechJournalReportEditValidator : AbstractValidator<TechJournalReportEditDto>
{
    public TechJournalReportEditValidator()
    {
        Include(new TechJournalReportCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}