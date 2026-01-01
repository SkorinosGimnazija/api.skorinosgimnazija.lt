namespace API.Endpoints.Posts.Public.List;

using API.Database.Entities.CMS;

public sealed class ListPostPublicMapper : ResponseMapper<ListPostPublicResponse, Post>
{
    public override ListPostPublicResponse FromEntity(Post e)
    {
        return new()
        {
            Id = e.Id,
            Slug = e.Slug,
            PublishedAt = e.PublishedAt,
            ModifiedAt = e.ModifiedAt,
            Title = e.Title,
            FeaturedImage = e.FeaturedImage,
            IntroText = e.IntroText
        };
    }
}