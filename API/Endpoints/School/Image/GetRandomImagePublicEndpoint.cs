namespace API.Endpoints.School.Image;

using API.Services.Settings;
using Microsoft.Extensions.Caching.Memory;

public sealed class GetRandomImagePublicEndpoint(
    AppDbContext dbContext,
    SettingsProvider settings,
    IMemoryCache cache)
    : EndpointWithoutRequest<PostRandomImageResponse>
{
    private const string CacheKey = "random-image";

    public override void Configure()
    {
        Get("public/school/random-image");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var imageData = await cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            var imageSettings = await settings.GetRandomImageSettings();

            entry.AbsoluteExpirationRelativeToNow = imageSettings.CacheDuration;

            var query = dbContext.Posts.AsNoTracking().AsQueryable();

            if (imageSettings.ForcedPostId is not null)
            {
                query = query.Where(x => x.Id == imageSettings.ForcedPostId);
            }
            else
            {
                query = query.Where(x =>
                        x.IsPublished &&
                        x.ShowInFeed &&
                        x.Images != null)
                    .OrderBy(x => EF.Functions.Random());
            }

            var post = await query.FirstOrDefaultAsync(ct);
            if (post?.Images is null || post.Images.Count == 0)
            {
                return null;
            }

            var imageIndex = Random.Shared.Next(post.Images.Count);
            var imageUrl = post.Images[imageIndex];

            return new PostRandomImageResponse
            {
                Url = imageUrl,
                PostId = post.Id,
                PostTitle = post.Title,
                PostDate = post.PublishedAt
            };
        });

        if (imageData is null)
        {
            await Send.NoContentAsync(ct);
            return;
        }

        await Send.OkAsync(imageData, ct);
    }
}