using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Menus.Validation
{
using Application.Menus.Dtos;
    using Domain.CMS;
    using FluentValidation;

    public class MenuEditValidator : AbstractValidator<MenuEditDto>
    {
        public MenuEditValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.DomainId).NotEmpty();
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
}
