namespace API.Services.Storage;

public interface IStorageService
{
    Task<StorageFile?> GetFileAsync(string fileId);

    Task<bool> DeleteFileAsync(string fileId);

    Task<IList<string>> GenerateIdsAsync(int count);

    Task<string> GenerateIdAsync();

    Task<string> SaveFileAsync(IFormFile file);

    Task<string> SaveJpgImageStreamAsync(Stream stream);

    Task<string> SaveStreamAsync(Stream stream, UploadStreamMetadata metadata);
}