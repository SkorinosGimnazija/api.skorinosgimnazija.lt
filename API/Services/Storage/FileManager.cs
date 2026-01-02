namespace API.Services.Storage;

using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using API.Services.ImageOptimization;
using API.Services.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;

public partial class FileManager(
    IOptions<UrlOptions> urls,
    ILogger<FileManager> logger,
    IImageOptimizer imageOptimizer,
    IHttpClientFactory httpClientFactory,
    IStorageService storageService,
    IMemoryCache cache)
{
    private static readonly Regex FileUrlReplaceTemplateRegex = FileLinkRegex();

    [GeneratedRegex("{{generate-link-([^}]+)}}", RegexOptions.Compiled)]
    private static partial Regex FileLinkRegex();

    public async Task<string> SaveImage(IFormFile file, bool optimize, bool isFeatured = true)
    {
        return (await SaveImages([file], optimize, isFeatured)).First().Value;
    }

    public async Task<IReadOnlyDictionary<string, string>> SaveImages(
        List<IFormFile> files, bool optimize, bool isFeatured = false)
    {
        if (!optimize)
        {
            return await SaveFiles(files);
        }

        var savedFiles = new ConcurrentDictionary<string, string>();
        var randomTag = Path.GetRandomFileName();
        using var client = httpClientFactory.CreateClient();

        await Parallel.ForEachAsync(files,
            new ParallelOptions { MaxDegreeOfParallelism = 10 },
            async (file, ct) =>
            {
                try
                {
                    var url = await imageOptimizer.OptimizeAsync(file, randomTag, isFeatured);
                    await using var stream = await client.GetStreamAsync(url, ct);
                    var fileId = await storageService.SaveJpgImageStreamAsync(stream);

                    savedFiles.TryAdd(file.FileName, fileId);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error occurred while saving image");
                }
            });

        await new DeleteOptimizedImageCommand { ImageTag = randomTag }.QueueJobAsync();

        if (savedFiles.Count != files.Count)
        {
            await new DeleteFileCommand { FileIds = savedFiles.Values }.QueueJobAsync();
            throw new("Error occurred while saving images");
        }

        return savedFiles;
    }

    public async Task<string> SaveFile(IFormFile file)
    {
        return (await SaveFiles([file])).First().Value;
    }

    public async Task<IReadOnlyDictionary<string, string>> SaveFiles(List<IFormFile> files)
    {
        var savedFiles = new ConcurrentDictionary<string, string>();

        await Parallel.ForEachAsync(files,
            new ParallelOptions { MaxDegreeOfParallelism = 10 },
            async (file, _) =>
            {
                try
                {
                    var fileId = await storageService.SaveFileAsync(file);
                    savedFiles.TryAdd(file.FileName, fileId);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Error occurred while saving file");
                }
            });

        if (savedFiles.Count != files.Count)
        {
            await new DeleteFileCommand { FileIds = savedFiles.Values }.QueueJobAsync();
            throw new("Error occurred while saving files");
        }

        return savedFiles;
    }

    public string? ReplaceTextLinks(string? entityText, IReadOnlyDictionary<string, string> files)
    {
        if (entityText is null || files.Count == 0)
        {
            return entityText;
        }

        return FileUrlReplaceTemplateRegex.Replace(entityText, match =>
        {
            var fileName = match.Groups[1].Value;
            if (!files.TryGetValue(fileName, out var fileId))
            {
                return match.Value;
            }

            return $"{urls.Value.Static}/{fileId}";
        });
    }

    public async Task<StorageFile?> GetFileAsync(string fileId)
    {
        var key = $"lf-{fileId}";
        var localFilePath = Path.Combine(Path.GetTempPath(), key);
        var metadata = await cache.GetOrCreateAsync(key, async entry =>
        {
            var file = await storageService.GetFileAsync(fileId);
            if (file is null)
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60);
                return null;
            }

            await File.WriteAllBytesAsync(localFilePath, file.Data);

            return new FileMetadata
            {
                Id = file.Id,
                Name = file.Name,
                ContentType = file.ContentType
            };
        });

        if (metadata is null)
        {
            return null;
        }

        return new()
        {
            Id = metadata.Id,
            Name = metadata.Name,
            ContentType = metadata.ContentType,
            Data = await File.ReadAllBytesAsync(localFilePath)
        };
    }

    public async Task<bool> DeleteFileAsync(string fileId)
    {
        return await storageService.DeleteFileAsync(fileId);
    }

    public (int Width, int Height) GetImageDimensions(IFormFile image)
    {
        using var imageStream = image.OpenReadStream();
        using var imageInfo = Image.Load(imageStream);

        return (imageInfo.Width, imageInfo.Height);
    }

    public List<string> GetLinkedFiles(string? entityText, IEnumerable<string> existingFiles)
    {
        if (string.IsNullOrWhiteSpace(entityText))
        {
            return [];
        }

        return existingFiles.Where(entityText.Contains).ToList();
    }
}