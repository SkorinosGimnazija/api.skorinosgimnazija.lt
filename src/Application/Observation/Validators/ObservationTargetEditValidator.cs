namespace SkorinosGimnazija.Application.Observation.Validators;

using Dtos;
using FluentValidation;

public class ObservationTargetEditValidator : AbstractValidator<ObservationTargetEditDto>
{
    public ObservationTargetEditValidator()
    {
        Include(new ObservationTargetCreateValidator());
        RuleFor(v => v.Id).NotEmpty();
    }
    
}