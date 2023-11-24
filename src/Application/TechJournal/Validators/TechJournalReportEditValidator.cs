namespace SkorinosGimnazija.Application.TechJournal.Validators;

using FluentValidation;
using SkorinosGimnazija.Application.TechJournal.Dtos;

internal class TechJournalReportEditValidator : AbstractValidator<TechJournalReportEditDto>
{
    public TechJournalReportEditValidator()
    {
        Include(new TechJournalReportCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}