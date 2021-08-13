using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Menus.Validation
{
    using Domain.CMS;
    using FluentValidation;

    public class MenuCreateValidator : AbstractValidator<MenuCreateDto>
    {
        public MenuCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.DomainId).NotEmpty();
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}
