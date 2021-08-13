namespace Application.Posts.Validation
{
    using Dtos;
    using FluentValidation;

    public  class PostEditValidator : AbstractValidator<PostEditDto>
    {
        public PostEditValidator()
        { 
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Slug).NotEmpty();
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.DomainId).NotEmpty();
        }
    }
}
