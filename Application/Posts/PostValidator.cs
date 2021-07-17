using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Posts
{
    using Domain.CMS;
    using FluentValidation;

    public  class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(x => x.Title).NotEmpty();

        }
    }
}
