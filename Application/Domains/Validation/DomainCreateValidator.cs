using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Domains.Validation
{
    using Domain.CMS;
    using Dtos;
    using FluentValidation;

    public  class DomainCreateValidator : AbstractValidator<DomainCreateDto>
    {
        public DomainCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Slug).NotEmpty();
        }
    }
}
