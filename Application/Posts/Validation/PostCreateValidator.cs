namespace Application.Posts.Validation
{
    using Dtos;
    using FluentValidation;

    public  class PostCreateValidator : AbstractValidator<PostCreateDto>
    {
        public PostCreateValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Slug).NotEmpty();
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.DomainId).NotEmpty();
        }
    }
}
