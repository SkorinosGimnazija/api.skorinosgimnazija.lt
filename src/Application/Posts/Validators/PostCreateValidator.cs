namespace SkorinosGimnazija.Application.Posts.Validators;

using System.Data;
using Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Http;

public class PostCreateValidator : AbstractValidator<PostCreateDto>
{
    public PostCreateValidator()
    {
        RuleFor(x => x.PublishedAt).NotEmpty();
        RuleFor(x => x.LanguageId).NotEmpty();
        RuleFor(x => x.Meta).MaximumLength(256);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(256);
        RuleFor(x => x.NewFiles).Must(BeUnique).WithMessage("File names must be unique");
        RuleFor(x => x.Slug).NotEmpty().MaximumLength(256).DependentRules(() =>
        {
            RuleFor(x => x.Slug).Must(x => !x.Contains('/')).WithMessage("Forbidden char '/'");
        }); 
    }
      
    private static bool BeUnique(PostCreateDto post, IFormFileCollection? files)
    {
        if (files is null)
        {
            return true;
        }

        return files.DistinctBy(x => x.FileName).Count() == files.Count;
    }
}