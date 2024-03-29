﻿namespace SkorinosGimnazija.Infrastructure.FileManagement;

using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Application.Common.Interfaces;
using Domain.Options;
using ImageOptimization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

public sealed class MediaManager : IMediaManager
{
    public const string FileUrlReplaceTemplate = "{auto-link}";

    private static readonly Regex FileUrlReplaceTemplateRegex =
        new($"(\\(|\"){FileUrlReplaceTemplate}/(.*?)(\\)|\")", RegexOptions.Compiled, TimeSpan.FromSeconds(3));

    private readonly string _baseUploadPath;
    private readonly IFileService _fileService;
    private readonly IImageOptimizer _imageOptimizer;
    private readonly string _staticFilesUploadUrl;

    public MediaManager(
        IOptions<MediaManagerOptions> options,
        IOptions<UrlOptions> urls,
        IImageOptimizer imageOptimizer,
        IFileService fileService)
    {
        _baseUploadPath = Path.Combine(options.Value.UploadPath);
        _staticFilesUploadUrl = urls.Value.Static;
        _imageOptimizer = imageOptimizer;
        _fileService = fileService;
    }

    public async Task<List<string>> SaveImagesAsync(
        IEnumerable<IFormFile> files, bool optimize, bool featuredImageProfile)
    {
        if (!optimize)
        {
            return await SaveFilesAsync(files);
        }

        var randomName = Path.GetRandomFileName();
        var directory = Directory.CreateDirectory(Path.Combine(_baseUploadPath, randomName));
        var savedFiles = new ConcurrentBag<string>();

        try
        {
            await Parallel.ForEachAsync(files,
                new ParallelOptions { MaxDegreeOfParallelism = 8 },
                async (file, _) =>
                {
                    var imageUrl = await _imageOptimizer.OptimizeAsync(file, directory.Name, featuredImageProfile);
                    var savedImage = await _fileService.DownloadFileAsync(imageUrl, directory.FullName);

                    savedFiles.Add($"{directory.Name}/{savedImage}");
                });
        }
        catch
        {
            DeleteFiles(savedFiles);
            throw;
        }
        finally
        {
            await _imageOptimizer.DeleteFilesAsync(directory.Name);
        }

        return savedFiles.ToList();
    }

    public async Task<List<string>> SaveFilesAsync(IEnumerable<IFormFile> files)
    {
        var randomName = Path.GetRandomFileName();
        var directory = Directory.CreateDirectory(Path.Combine(_baseUploadPath, randomName));
        var savedFiles = new List<string>();

        try
        {
            foreach (var file in files)
            {
                await _fileService.SaveFileAsync(file, directory.FullName);
                savedFiles.Add($"{directory.Name}/{file.FileName}");
            }
        }
        catch
        {
            DeleteFiles(savedFiles);
            throw;
        }

        return savedFiles;
    }

    public string? GenerateFileLinks(string? text, ICollection<string>? files)
    {
        if (text is null || files is null)
        {
            return text;
        }

        return FileUrlReplaceTemplateRegex.Replace(text, x =>
        {
            var path = files.FirstOrDefault(z => z.EndsWith(x.Groups[2].Value));
            if (path is null)
            {
                return x.Value;
            }

            var slug = string.Join("/", path.Split("/").Select(Uri.EscapeDataString));
            var openChar = x.Groups[1].Value;
            var closeChar = x.Groups[3].Value;

            return $"{openChar}{_staticFilesUploadUrl}/{slug}{closeChar}";
        });
    }

    public void DeleteFiles(string? file)
    {
        if (file is null)
        {
            return;
        }

        DeleteFile(file);
    }

    public void DeleteFiles(IEnumerable<string>? files)
    {
        if (files is null)
        {
            return;
        }

        foreach (var file in files)
        {
            DeleteFile(file);
        }
    }

    public void DeleteFile(string file)
    {
        _fileService.DeleteFile(Path.Combine(_baseUploadPath, file));
    }
}