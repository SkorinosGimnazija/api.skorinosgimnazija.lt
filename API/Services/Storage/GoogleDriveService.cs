namespace API.Services.Storage;

using System.Net;
using API.Services.Options;
using Google;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Upload;
using Microsoft.Extensions.Options;

public class GoogleDriveService(
    IOptions<GoogleOptions> googleOptions,
    IOptions<GoogleDriveOptions> googleDriveOptions)
    : IStorageService
{
    private readonly DriveService _driveService = new(new()
    {
        HttpClientInitializer = googleOptions.Value.CreateCredential()
            .CreateScoped(DriveService.ScopeConstants.Drive)
            .CreateWithUser(googleDriveOptions.Value.User)
    });

    public async Task<StorageFile?> GetFileAsync(string fileId)
    {
        try
        {
            var request = _driveService.Files.Get(fileId);
            request.SupportsAllDrives = true;
            request.Fields = "id, name, mimeType";

            var metadata = await request.ExecuteAsync();

            using var stream = new MemoryStream();
            await request.DownloadAsync(stream);

            return new()
            {
                Id = metadata.Id,
                Name = metadata.Name,
                ContentType = metadata.MimeType,
                Data = stream.ToArray()
            };
        }
        catch (GoogleApiException e) when
            (e.HttpStatusCode is HttpStatusCode.NotFound or HttpStatusCode.Gone)
        {
            return null;
        }
    }

    public async Task<bool> DeleteFileAsync(string fileId)
    {
        try
        {
            var request = _driveService.Files.Update(new() { Trashed = true }, fileId);
            request.SupportsAllDrives = true;

            await request.ExecuteAsync();

            return true;
        }
        catch (GoogleApiException e) when
            (e.HttpStatusCode is HttpStatusCode.NotFound or HttpStatusCode.Gone)
        {
            return false;
        }
    }

    public async Task<IList<string>> GenerateIdsAsync(int count)
    {
        var request = _driveService.Files.GenerateIds();
        request.Count = count;

        var result = await request.ExecuteAsync();

        return result.Ids;
    }

    public async Task<string> GenerateIdAsync()
    {
        return (await GenerateIdsAsync(1))[0];
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var fileMetadata = new UploadStreamMetadata
        {
            FileExtension = Path.GetExtension(file.FileName),
            MimeType = file.ContentType
        };

        await using var stream = file.OpenReadStream();
        return await SaveStreamAsync(stream, fileMetadata);
    }

    public async Task<string> SaveJpgImageStreamAsync(Stream stream)
    {
        var fileMetadata = new UploadStreamMetadata
        {
            FileExtension = "jpg",
            MimeType = "image/jpeg"
        };

        return await SaveStreamAsync(stream, fileMetadata);
    }

    public async Task<string> SaveStreamAsync(Stream stream, UploadStreamMetadata metadata)
    {
        var fileId = await GenerateIdAsync();
        var fileMetadata = new File
        {
            Id = fileId,
            Name = Path.ChangeExtension(fileId, metadata.FileExtension),
            MimeType = metadata.MimeType,
            Parents = [googleDriveOptions.Value.FolderId]
        };

        var request = _driveService.Files.Create(fileMetadata, stream, metadata.MimeType);
        request.SupportsAllDrives = true;

        var progress = await request.UploadAsync();

        if (progress.Status != UploadStatus.Completed)
        {
            throw progress.Exception ?? new("File upload failed");
        }

        return fileId;
    }
}