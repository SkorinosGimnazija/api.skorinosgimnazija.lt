namespace SkorinosGimnazija.Application.Menus.Validators;

using Dtos;
using FluentValidation;

internal class MenuCreateValidator : AbstractValidator<MenuCreateDto>
{
    public MenuCreateValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Slug).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LanguageId).NotEmpty();
        RuleFor(x => x.MenuLocationId).NotEmpty();
    }
}