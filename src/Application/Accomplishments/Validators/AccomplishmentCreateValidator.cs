namespace SkorinosGimnazija.Application.Accomplishments.Validators;

using Dtos;
using FluentValidation;

internal class AccomplishmentCreateValidator : AbstractValidator<AccomplishmentCreateDto>
{
    public AccomplishmentCreateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.ScaleId).GreaterThan(0);
        RuleForEach(x => x.Students)
            .ChildRules(s =>
            {
                s.RuleFor(x => x.Name).MaximumLength(128);
                s.RuleFor(x => x.ClassroomId).GreaterThan(0);
                s.RuleFor(x => x.AchievementId).GreaterThan(0);
            });
        RuleForEach(x => x.AdditionalTeachers)
            .ChildRules(s => { s.RuleFor(x => x.Name).MaximumLength(128); });
    }
}