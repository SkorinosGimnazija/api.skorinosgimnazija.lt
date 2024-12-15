namespace SkorinosGimnazija.Application.Observation.Validators;

using Dtos;
using FluentValidation;

public class StudentObservationEditValidator : AbstractValidator<StudentObservationEditDto>
{
    public StudentObservationEditValidator()
    {
        Include(new StudentObservationCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}