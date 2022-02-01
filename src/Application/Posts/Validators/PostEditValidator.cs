namespace SkorinosGimnazija.Application.Posts.Validators;

using Dtos;
using FluentValidation;

public class PostEditValidator : AbstractValidator<PostEditDto>
{
    public PostEditValidator()
    {
        Include(new PostCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
    }
}