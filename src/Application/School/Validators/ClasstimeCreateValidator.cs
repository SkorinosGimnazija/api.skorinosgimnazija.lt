namespace SkorinosGimnazija.Application.School.Validators;

using Dtos;
using FluentValidation;

internal class ClasstimeCreateValidator : AbstractValidator<ClasstimeCreateDto>
{
    public ClasstimeCreateValidator()
    {
        RuleFor(x => x.Number).NotNull();
        RuleFor(x => x.StartTime).NotNull();
        RuleFor(x => x.EndTime).NotNull().GreaterThan(x => x.StartTime);
    }
}