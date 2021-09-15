namespace Infrastructure.FileManager
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using ImageOptimization;
    using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    public sealed class FileManager : IFileManager,  IDisposable
    {
        private readonly IImageOptimizer _imageOptimizer;
        private readonly string _baseUploadPath;
        private readonly HttpClient _httpClient = new();

        public FileManager(IConfiguration config, IImageOptimizer imageOptimizer)
        {
            _imageOptimizer = imageOptimizer;
            _baseUploadPath = config["FILE_UPLOAD_PATH"];
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

      

        public async Task<List<string>> SaveImagesAsync(int postId, IEnumerable<IFormFile> files)
        {
            var directory = Directory.CreateDirectory(Path.Combine(_baseUploadPath, postId.ToString()));
            var savedImages = new ConcurrentBag<string>();

            await Parallel.ForEachAsync(files, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (file, ct) =>
            {
                var optimizedImage = await _imageOptimizer.OptimizeAsync(file);
                var savedImage = await DownloadFileAsync(optimizedImage.Url, directory.FullName);
                await _imageOptimizer.DeleteAsync(optimizedImage.Id);
                
                savedImages.Add(savedImage);
            });
            
            return savedImages.ToList();
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

        public Task DeleteFilesAsync(int postId, List<string> files)
        {
            return Task.Run(() =>
            {
                var folderPath = Path.Combine(_baseUploadPath, postId.ToString());

                foreach (var file in files)
                {
                    File.Delete(Path.Combine(folderPath, file));
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
    }
}