namespace SkorinosGimnazija.Application.Observation.Validators;

using Dtos;
using FluentValidation;

public class ObservationTargetCreateValidator : AbstractValidator<ObservationTargetCreateDto>
{
    public ObservationTargetCreateValidator()
    {
        RuleFor(v => v.Name).NotEmpty().MaximumLength(255);
    }
}