namespace SkorinosGimnazija.Application.School.Validators;

using Dtos;
using FluentValidation;

internal class ClasstimeShortDayCreateValidator : AbstractValidator<ClasstimeShortDayCreateDto>
{
    public ClasstimeShortDayCreateValidator()
    {
        RuleFor(x => x.Date).NotNull();
    }
}