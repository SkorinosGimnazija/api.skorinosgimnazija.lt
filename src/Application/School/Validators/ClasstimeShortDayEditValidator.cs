namespace SkorinosGimnazija.Application.School.Validators;

using Dtos;
using FluentValidation;

internal class ClasstimeShortDayEditValidator : AbstractValidator<ClasstimeShortDayEditDto>
{
    public ClasstimeShortDayEditValidator()
    {
        Include(new ClasstimeShortDayCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}