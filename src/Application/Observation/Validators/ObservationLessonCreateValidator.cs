namespace SkorinosGimnazija.Application.Observation.Validators;

using Dtos;
using FluentValidation;

public class ObservationLessonCreateValidator : AbstractValidator<ObservationLessonCreateDto>
{
    public ObservationLessonCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
    }
}