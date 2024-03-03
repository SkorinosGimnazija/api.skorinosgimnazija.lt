namespace SkorinosGimnazija.Application.BullyJournal.Validators;

using Dtos;
using FluentValidation;

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