namespace API.Endpoints.Posts.Create;

using API.Database.Entities.CMS;
using JetBrains.Annotations;

[PublicAPI]
public record CreatePostRequest
{
    public required bool IsFeatured { get; init; }

    public required bool IsPublished { get; init; }

    public required bool ShowInFeed { get; init; }

    public required bool OptimizeImages { get; init; }

    public required DateTime PublishedAt { get; init; }

    public DateTime? ModifiedAt { get; set; }

    public required string LanguageId { get; init; }

    public required string Title { get; init; }

    public required string Slug { get; init; }

    public string? IntroText { get; init; }

    public string? Text { get; init; }

    public IFormFile? NewFeaturedImage { get; init; }

    public string? Meta { get; init; }

    public List<IFormFile>? NewFiles { get; init; }

    public List<IFormFile>? NewImages { get; init; }
}

public class CreatePostRequestValidator : Validator<CreatePostRequest>
{
    private const long MaxImageSize = 10_000_000; // ~10mb

    public CreatePostRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(PostConfiguration.TitleLength);

        RuleFor(x => x.Slug)
            .NotEmpty()
            .MaximumLength(PostConfiguration.SlugLength)
            .DependentRules(() =>
            {
                RuleFor(x => x.Slug)
                    .Must(x => !x.Contains('/'))
                    .WithMessage("Forbidden char '/' for slug");
            });

        RuleFor(x => x.Meta)
            .MaximumLength(PostConfiguration.MetaLength);

        RuleFor(x => x.PublishedAt)
            .NotEmpty();

        RuleFor(x => x.NewFiles)
            .Must(BeUnique)
            .WithMessage("File names must be unique");

        RuleFor(x => x.NewImages)
            .Must(BeUnderSizeLimit)
            .WithMessage("Image size is too large");

        RuleFor(x => x.NewFeaturedImage)
            .Must(BeUnderSizeLimit)
            .WithMessage("Image size is too large");
    }

    private bool BeUnderSizeLimit(CreatePostRequest req, List<IFormFile>? images)
    {
        if (images is null)
        {
            return true;
        }

        return images.All(x => x.Length <= MaxImageSize);
    }

    private bool BeUnderSizeLimit(CreatePostRequest req, IFormFile? image)
    {
        if (image is null)
        {
            return true;
        }

        return image.Length <= MaxImageSize;
    }

    private bool BeUnique(CreatePostRequest req, List<IFormFile>? files)
    {
        if (files is null)
        {
            return true;
        }

        return files.DistinctBy(x => x.FileName).Count() == files.Count;
    }
}