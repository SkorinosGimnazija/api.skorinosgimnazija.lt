namespace SkorinosGimnazija.Application.Posts.Validators;

using Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Http;

public class PostCreateValidator : AbstractValidator<PostCreateDto>
{
    private const long MaxImageSize = 10485760;

    public PostCreateValidator()
    {
        RuleFor(x => x.PublishedAt).NotEmpty();
        RuleFor(x => x.LanguageId).NotEmpty();
        RuleFor(x => x.Meta).MaximumLength(256);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(256);
        RuleFor(x => x.NewFiles).Must(BeUnique).WithMessage("File names must be unique");
        RuleFor(x => x.NewImages).Must(BeUnderSizeLimit).WithMessage("Image size is too large");
        RuleFor(x => x.NewFeaturedImage).Must(BeUnderSizeLimit).WithMessage("Image size is too large");
        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(256)
            .DependentRules(() =>
            {
                RuleFor(x => x.Slug).Must(x => !x.Contains('/')).WithMessage("Forbidden char '/'");
            });
    }

    private static bool BeUnderSizeLimit(PostCreateDto post, IFormFileCollection? images)
    {
        if (images is null)
        {
            return true;
        }

        return images.All(x => x.Length < MaxImageSize);
    }

    private static bool BeUnderSizeLimit(PostCreateDto post, IFormFile? image)
    {
        if (image is null)
        {
            return true;
        }

        return image.Length < MaxImageSize;
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