namespace SkorinosGimnazija.Application.Observation.Validators;

using Dtos;
using FluentValidation;

public class StudentObservationCreateValidator : AbstractValidator<StudentObservationCreateDto>
{
    public StudentObservationCreateValidator()
    {
        RuleFor(x => x.Note).MaximumLength(500);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.TargetId).NotEmpty();
        RuleFor(x => x.LessonId).NotEmpty();
        RuleFor(x => x.TypeIds).NotNull();
    }
}