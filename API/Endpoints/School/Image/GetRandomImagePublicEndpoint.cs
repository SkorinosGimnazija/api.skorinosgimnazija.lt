namespace API.Endpoints.School.Image;

public sealed class GetRandomImagePublicEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<ImageResponse>
{
    public override void Configure()
    {
        Get("public/school/random-image");
        ResponseCache(300);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var images = await dbContext.Posts.AsNoTracking()
                         .Where(x =>
                             x.IsPublished &&
                             x.ShowInFeed &&
                             x.Images != null)
                         .OrderBy(x => EF.Functions.Random())
                         .Select(x => x.Images)
                         .FirstOrDefaultAsync(ct);

        if (images is null || images.Count == 0)
        {
            await Send.NoContentAsync(ct);
            return;
        }

        var imageIndex = Random.Shared.Next(images.Count);
        var imageUrl = images[imageIndex];

        await Send.OkAsync(new() { Url = imageUrl }, ct);
    }
}