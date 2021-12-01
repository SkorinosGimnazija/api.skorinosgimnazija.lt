namespace SkorinosGimnazija.Application.Posts.Validators;

using Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Http;

public class PostEditValidator : AbstractValidator<PostEditDto>
{
    public PostEditValidator()
    {
        Include(new PostCreateValidator());
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Files).Must(BeUnique).WithMessage("File names must be unique");
    }
     
    private static bool BeUnique(PostEditDto post, ICollection<string>? files)
    {
        if (files is null || post.NewFiles is null)
        {
            return true;
        }

        var allFileNames = files.Concat(post.NewFiles.Select(x => x.FileName)).ToList();

        return allFileNames.Distinct().Count() == allFileNames.Count;
    }
}