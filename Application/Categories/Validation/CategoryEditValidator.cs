using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Categories.Validation
{
    using Dtos;
    using FluentValidation;

    public   class CategoryEditValidator : AbstractValidator<CategoryEditDto>
    {
        public CategoryEditValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.LanguageId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Slug).NotEmpty();
        }
    }
}
