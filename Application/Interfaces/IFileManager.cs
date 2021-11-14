namespace Application.Interfaces;

using Microsoft.AspNetCore.Http;

public interface IFileManager
{
    Task<List<string>> SaveImagesAsync(int id, IFormFileCollection files);

    void DeleteFiles(List<string> files);

    void DeleteAllFiles(int postId);

    Task<List<string>> SaveFilesAsync(int postId, IFormFileCollection files);
}