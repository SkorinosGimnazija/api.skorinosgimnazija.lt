namespace SkorinosGimnazija.Application.Courses.Validators;

using Dtos;
using FluentValidation;

internal class CourseEditValidator : AbstractValidator<CourseEditDto>
{
    public CourseEditValidator()
    {
        Include(new CourseCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}