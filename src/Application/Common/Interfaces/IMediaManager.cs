namespace SkorinosGimnazija.Application.Common.Interfaces;

using Microsoft.AspNetCore.Http;

public interface IMediaManager
{
    Task<List<string>> SaveImagesAsync(IEnumerable<IFormFile> files, bool optimize);

    Task<List<string>> SaveFilesAsync(IEnumerable<IFormFile> files);

    void DeleteFiles(IEnumerable<string>? files);

    void DeleteFile(string file);

    string? GenerateFileLinks(string? text, ICollection<string>? files);
}