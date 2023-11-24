namespace SkorinosGimnazija.Application.School.Validators;

using Dtos;
using FluentValidation;

internal class ClassroomEditValidator : AbstractValidator<ClassroomEditDto>
{
    public ClassroomEditValidator()
    {
        Include(new ClassroomCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}