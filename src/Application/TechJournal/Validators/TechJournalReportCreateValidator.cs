namespace SkorinosGimnazija.Application.TechJournal.Validators;

using Dtos;
using FluentValidation;

internal class TechJournalReportCreateValidator : AbstractValidator<TechJournalReportCreateDto>
{
    public TechJournalReportCreateValidator()
    {
        RuleFor(x => x.Details).NotEmpty().MaximumLength(512);
        RuleFor(x => x.Place).NotEmpty().MaximumLength(64);
    }
}