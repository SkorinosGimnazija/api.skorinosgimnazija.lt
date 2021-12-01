namespace SkorinosGimnazija.Infrastructure.Services;

using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

public sealed class FileService : IFileService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public FileService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public void DeleteFolder(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, true);
        }
    }

    public async Task SaveFileAsync(IFormFile file, string directoryPath)
    {
        var filePath = Path.Combine(directoryPath, file.FileName);

        await using var stream = File.Create(filePath);
        await file.CopyToAsync(stream);
    }

    public async Task<string> DownloadFileAsync(Uri url, string directoryPath)
    {
        var uriWithoutQuery = url.GetLeftPart(UriPartial.Path);
        var fileName = Path.GetFileName(uriWithoutQuery);
        var filePath = Path.Combine(directoryPath, fileName);

        var httpClient = _httpClientFactory.CreateClient();
        var bytes = await httpClient.GetByteArrayAsync(url);

        await File.WriteAllBytesAsync(filePath, bytes);

        return fileName;
    }
}