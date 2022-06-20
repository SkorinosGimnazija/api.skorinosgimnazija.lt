namespace SkorinosGimnazija.Application.Accomplishments.Validators;

using Dtos;
using FluentValidation;

internal class AccomplishmentEditValidator : AbstractValidator<AccomplishmentEditDto>
{
    public AccomplishmentEditValidator()
    {
        Include(new AccomplishmentCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}