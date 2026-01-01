namespace API.Endpoints.Posts.Public.Get;

using API.Database.Entities.CMS;

public sealed class GetPostPublicMapper : ResponseMapper<GetPostPublicResponse, Post>
{
    public override GetPostPublicResponse FromEntity(Post e)
    {
        return new()
        {
            Id = e.Id,
            Slug = e.Slug,
            PublishedAt = e.PublishedAt,
            ModifiedAt = e.ModifiedAt,
            Title = e.Title,
            Text = e.Text,
            Meta = e.Meta,
            Images = e.Images
        };
    }
}