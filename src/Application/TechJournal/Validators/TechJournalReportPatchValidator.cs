namespace SkorinosGimnazija.Application.TechJournal.Validators;

using Dtos;
using FluentValidation;

internal class TechJournalReportPatchValidator : AbstractValidator<TechJournalReportPatchDto>
{
    public TechJournalReportPatchValidator()
    {
        RuleFor(x => x.Notes).MaximumLength(512);
    }
}