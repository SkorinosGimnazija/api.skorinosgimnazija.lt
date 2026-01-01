namespace API.Endpoints.Posts;

using API.Database.Entities.CMS;
using API.Endpoints.Posts.Create;
using API.Endpoints.Posts.Update;

public sealed class PostMapper
    : Mapper<CreatePostRequest, PostResponse, Post>
{
    public override Post ToEntity(CreatePostRequest r)
    {
        return new()
        {
            IsFeatured = r.IsFeatured,
            IsPublished = r.IsPublished,
            ShowInFeed = r.ShowInFeed,
            PublishedAt = r.PublishedAt.ToUniversalTime(),
            ModifiedAt = r.ModifiedAt?.ToUniversalTime(),
            LanguageId = r.LanguageId,
            Title = r.Title.Trim(),
            Slug = r.Slug.Trim(),
            IntroText = r.IntroText?.ReplaceLineEndings("\n"),
            Text = r.Text?.ReplaceLineEndings("\n"),
            Meta = string.IsNullOrWhiteSpace(r.Meta) ? null : r.Meta.Trim()
        };
    }

    public Post UpdateEntity(UpdatePostRequest r, Post e)
    {
        e.IsFeatured = r.IsFeatured;
        e.IsPublished = r.IsPublished;
        e.ShowInFeed = r.ShowInFeed;
        e.PublishedAt = r.PublishedAt.ToUniversalTime();
        e.ModifiedAt = r.ModifiedAt?.ToUniversalTime();
        e.LanguageId = r.LanguageId;
        e.Title = r.Title.Trim();
        e.Slug = r.Slug.Trim();
        e.IntroText = r.IntroText?.ReplaceLineEndings("\n");
        e.Text = r.Text?.ReplaceLineEndings("\n");
        e.Meta = string.IsNullOrWhiteSpace(r.Meta) ? null : r.Meta.Trim();

        return e;
    }

    public override PostResponse FromEntity(Post e)
    {
        return new()
        {
            Id = e.Id,
            IsFeatured = e.IsFeatured,
            IsPublished = e.IsPublished,
            ShowInFeed = e.ShowInFeed,
            PublishedAt = e.PublishedAt,
            ModifiedAt = e.ModifiedAt,
            LanguageId = e.LanguageId,
            Language = e.Language.Name,
            FeaturedImage = e.FeaturedImage,
            Title = e.Title,
            Slug = e.Slug,
            IntroText = e.IntroText,
            Text = e.Text,
            Meta = e.Meta,
            Files = e.Files,
            Images = e.Images
        };
    }
}