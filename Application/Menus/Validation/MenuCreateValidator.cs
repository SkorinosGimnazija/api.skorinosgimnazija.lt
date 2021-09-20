namespace Application.Menus.Validation
{
    using Dtos;
    using FluentValidation;

    public class MenuCreateValidator : AbstractValidator<MenuCreateDto>
    {
        public MenuCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}