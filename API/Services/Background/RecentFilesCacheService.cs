namespace API.Services.Background;

using System.Diagnostics;
using API.Services.Options;
using API.Services.Storage;
using Microsoft.Extensions.Options;

public sealed class RecentFilesCacheService(
    IServiceProvider services,
    ILogger<RecentFilesCacheService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        try
        {
            await CacheFilesAsync(ct);
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Task CacheFilesAsync(CancellationToken ct)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var fileManager = scope.ServiceProvider.GetRequiredService<FileManager>();
        var urls = scope.ServiceProvider.GetRequiredService<IOptions<UrlOptions>>();

        var sw = Stopwatch.StartNew();

        var bannerIds = await GetBannerImageIds(dbContext);
        var postIds = await GetPostFileIds(dbContext);
        var menuIds = await GetMenuFileIds(dbContext, urls);

        var fileIds = bannerIds
            .Union(postIds)
            .Union(menuIds)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToList();

        await Parallel.ForEachAsync(fileIds,
            new ParallelOptions { CancellationToken = ct, MaxDegreeOfParallelism = 4 },
            async (fileId, _) =>
            {
                try
                {
                    await fileManager.GetFileAsync(fileId);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error occurred while caching file {FileId}", fileId);
                }
            });

        sw.Stop();
        logger.LogInformation("Cached {Count} files in {Elapsed}", fileIds.Count, sw.Elapsed);
    }

    private async Task<List<string>> GetPostFileIds(AppDbContext dbContext)
    {
        var fileIds = new List<string>();
        var languages = await dbContext.Languages.AsNoTracking()
                            .Select(x => x.Id)
                            .ToListAsync();

        foreach (var languageId in languages)
        {
            var posts = await dbContext.Posts.AsNoTracking()
                            .Where(x => x.LanguageId == languageId && x.IsPublished && x.ShowInFeed)
                            .OrderByDescending(x => x.PublishedAt)
                            .Take(15)
                            .ToListAsync();

            fileIds.AddRange(posts.SelectMany(post =>
                new[] { post.FeaturedImage }
                    .Concat(post.Images ?? [])
                    .Concat(post.Files ?? [])
                    .OfType<string>()));
        }

        return fileIds;
    }

    private async Task<List<string>> GetBannerImageIds(AppDbContext dbContext)
    {
        return await dbContext.Banners.AsNoTracking()
                   .Where(x => x.IsPublished)
                   .Select(x => x.ImageUrl)
                   .ToListAsync();
    }

    private async Task<List<string>> GetMenuFileIds(
        AppDbContext dbContext, IOptions<UrlOptions> urls)
    {
        var staticLink = $"{urls.Value.Static}/";

        var fileIds = await dbContext.Menus.AsNoTracking()
                          .Where(x =>
                              x.IsPublished &&
                              x.IsExternal &&
                              x.Url != null &&
                              x.Url.StartsWith(staticLink))
                          .Select(x => x.Url!.Substring(staticLink.Length))
                          .ToListAsync();

        var menuPosts = await dbContext.Menus.AsNoTracking()
                            .Where(x => x.IsPublished && x.PostId != null)
                            .Select(x => x.Post!)
                            .ToListAsync();

        fileIds.AddRange(menuPosts.SelectMany(post =>
            new[] { post.FeaturedImage }
                .Concat(post.Images ?? [])
                .Concat(post.Files ?? [])
                .OfType<string>()));

        return fileIds;
    }
}