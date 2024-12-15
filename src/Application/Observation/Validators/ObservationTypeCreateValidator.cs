namespace SkorinosGimnazija.Application.Observation.Validators;

using Dtos;
using FluentValidation;

public class ObservationTypeCreateValidator: AbstractValidator<ObservationTypeCreateDto>
{
    public ObservationTypeCreateValidator()
    {
        RuleFor(v => v.Name).NotEmpty().MaximumLength(255);
    }
}