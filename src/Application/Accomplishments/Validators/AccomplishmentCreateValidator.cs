namespace SkorinosGimnazija.Application.Accomplishments.Validators;

using Dtos;
using FluentValidation;

internal class AccomplishmentCreateValidator : AbstractValidator<AccomplishmentCreateDto>
{
    public AccomplishmentCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Achievement).NotEmpty().MaximumLength(512);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.ScaleId).GreaterThan(0);
    }
}