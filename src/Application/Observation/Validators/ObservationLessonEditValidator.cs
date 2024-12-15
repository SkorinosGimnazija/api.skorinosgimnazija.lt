namespace SkorinosGimnazija.Application.Observation.Validators;

using Dtos;
using FluentValidation;

public class ObservationLessonEditValidator : AbstractValidator<ObservationLessonEditDto>
{
    
    public ObservationLessonEditValidator()
    {
        Include(new ObservationLessonCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}