namespace SkorinosGimnazija.Application.Observation.Validators;

using Dtos;
using FluentValidation;

public class ObservationTypeEditValidator: AbstractValidator<ObservationTypeEditDto>
{
    public ObservationTypeEditValidator()
    {
        Include(new ObservationTypeCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}