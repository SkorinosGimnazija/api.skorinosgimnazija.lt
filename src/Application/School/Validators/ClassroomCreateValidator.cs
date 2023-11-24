namespace SkorinosGimnazija.Application.School.Validators;

using Dtos;
using FluentValidation;

internal class ClassroomCreateValidator : AbstractValidator<ClassroomCreateDto>
{
    public ClassroomCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(48);
        RuleFor(x => x.Number).NotNull();
    }
}