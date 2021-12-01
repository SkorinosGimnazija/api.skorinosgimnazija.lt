namespace SkorinosGimnazija.Application.Menus.Validators;

using Dtos;
using FluentValidation;

internal class MenuEditValidator : AbstractValidator<MenuEditDto>
{
    public MenuEditValidator()
    {
        Include(new MenuCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ParentMenuId).NotEqual(x => x.Id);
    }
}