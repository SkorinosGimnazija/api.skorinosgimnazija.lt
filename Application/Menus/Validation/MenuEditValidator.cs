namespace Application.Menus.Validation
{
    using Dtos;
    using FluentValidation;

    public class MenuEditValidator : AbstractValidator<MenuEditDto>
    {
        public MenuEditValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}