namespace API.Endpoints.School.Image;

public sealed class GetRandomImagePublicEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<PostRandomImageResponse>
{
    public override void Configure()
    {
        Get("public/school/random-image");
        ResponseCache(300);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var post = await dbContext.Posts.AsNoTracking()
                       .Where(x =>
                           x.IsPublished &&
                           x.ShowInFeed &&
                           x.Images != null)
                       .OrderBy(x => EF.Functions.Random())
                       .FirstOrDefaultAsync(ct);

        if (post is null || post.Images!.Count == 0)
        {
            await Send.NoContentAsync(ct);
            return;
        }

        var imageIndex = Random.Shared.Next(post.Images.Count);
        var imageUrl = post.Images[imageIndex];

        await Send.OkAsync(new()
        {
            Url = imageUrl,
            PostId = post.Id,
            PostTitle = post.Title,
            PostDate = post.PublishedAt
        }, ct);
    }
}