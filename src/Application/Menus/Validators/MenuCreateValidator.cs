namespace SkorinosGimnazija.Application.Menus.Validators;

using Dtos;
using FluentValidation;

internal class MenuCreateValidator : AbstractValidator<MenuCreateDto>
{
    public MenuCreateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(256);
        RuleFor(x => x.LanguageId).NotEmpty();
        RuleFor(x => x.MenuLocationId).NotEmpty();
        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(100)
            .DependentRules(() =>
            {
                RuleFor(x => x.Slug).Must(x => !x.Contains('/')).WithMessage("Forbidden char '/'");
            });
        RuleFor(x => x.Url).Null().When(x => x.LinkedPostId is not null).WithMessage("Set url or post id");
    }
}