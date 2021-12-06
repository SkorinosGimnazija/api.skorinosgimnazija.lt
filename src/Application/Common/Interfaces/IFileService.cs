namespace SkorinosGimnazija.Application.Common.Interfaces;

using Microsoft.AspNetCore.Http;

public interface IFileService
{
    Task<string> DownloadFileAsync(Uri url, string directoryPath);

    Task SaveFileAsync(IFormFile file, string directoryPath);

    void DeleteFile(string filePath);

    void DeleteFolder(string folderPath);

    void DeleteEmptyFileFolder(string filePath);
}