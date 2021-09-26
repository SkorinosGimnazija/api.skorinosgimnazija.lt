namespace Infrastructure.FileManager
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using ImageOptimization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    public sealed class FileManager : IFileManager, IDisposable
    {
        private readonly string _baseUploadPath;
        private readonly HttpClient _httpClient = new();
        private readonly IImageOptimizer _imageOptimizer;

        public FileManager(IOptions<FileManagerSettings> options, IImageOptimizer imageOptimizer)
        {
            _baseUploadPath = options.Value.UploadPath;
            _imageOptimizer = imageOptimizer;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public async Task<List<string>> SaveImagesAsync(int postId, IFormFileCollection files)
        {
            var directory = Directory.CreateDirectory(Path.Combine(_baseUploadPath, postId.ToString()));
            var savedImages = new ConcurrentBag<string>();

            await Parallel.ForEachAsync(files, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (file, _) =>
            {
                var optimizedImage = await _imageOptimizer.OptimizeAsync(file);
                var savedImage = await DownloadFileAsync(optimizedImage.Url, directory.FullName);
                await _imageOptimizer.DeleteAsync(optimizedImage.Id);

                savedImages.Add($"{directory.Name}/{savedImage}");
            });

            return savedImages.ToList();
        }

        public async Task<List<string>> SaveFilesAsync(int postId, IFormFileCollection files)
        {
            var directory = Directory.CreateDirectory(Path.Combine(_baseUploadPath, postId.ToString()));
            var savedFiles = new List<string>();

            foreach (var file in files)
            {
                var filePath = Path.Combine(directory.FullName, file.FileName);

                await using var stream = File.Create(filePath);
                await file.CopyToAsync(stream);

                savedFiles.Add($"{directory.Name}/{file.FileName}");
            }

            return savedFiles;
        }

        public Task DeleteFilesAsync(List<string> files)
        {
            return Task.Run(() =>
            {
                foreach (var file in files)
                {
                    File.Delete(Path.Combine(_baseUploadPath, file));
                }
            });
        }

        public Task DeleteAllFilesAsync(int postId)
        {
            return Task.Run(() =>
            {
                var folderPath = Path.Combine(_baseUploadPath, postId.ToString());

                if (Directory.Exists(folderPath))
                {
                    Directory.Delete(folderPath, true);
                }
            });
        }

        private async Task<string> DownloadFileAsync(Uri url, string directoryPath)
        {
            var uriWithoutQuery = url.GetLeftPart(UriPartial.Path);
            var fileName = Path.GetFileName(uriWithoutQuery);
            var filePath = Path.Combine(directoryPath, fileName);

            var bytes = await _httpClient.GetByteArrayAsync(url);
            await File.WriteAllBytesAsync(filePath, bytes);

            return fileName;
        }
    }
}