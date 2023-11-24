namespace SkorinosGimnazija.Application.School.Validators;

using Dtos;
using FluentValidation;

internal class ClasstimeEditValidator : AbstractValidator<ClasstimeEditDto>
{
    public ClasstimeEditValidator()
    {
        Include(new ClasstimeCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}