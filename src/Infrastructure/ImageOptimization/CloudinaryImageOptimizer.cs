namespace SkorinosGimnazija.Infrastructure.ImageOptimization;

using Application.Common.Exceptions;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Options;

public sealed class CloudinaryImageOptimizer : IImageOptimizer
{
    private readonly Cloudinary _cloudinary;
    private readonly Transformation _defaultTransformation;
    private readonly ILogger<CloudinaryImageOptimizer> _logger;

    public CloudinaryImageOptimizer(
        ILogger<CloudinaryImageOptimizer> logger,
        IOptions<CloudinaryOptions> options,
        IHostEnvironment env)
    {
        _logger = logger;
        _cloudinary = new(options.Value.Url);
        _defaultTransformation = new Transformation()
            .Quality("90")
            .Gravity("face")
            .AspectRatio("16:9")
            .Width("1600")
            .Crop("fill")
            .Effect("improve")
            //.Effect(env.IsDevelopment() ? "improve" : "viesus_correct")
            .FetchFormat("jpg");
    }

    public async Task<Uri> OptimizeAsync(IFormFile image, string directoryName)
    {
        await using var stream = image.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            Transformation = _defaultTransformation,
            File = new(image.FileName, stream),
            Tags = directoryName
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error is not null)
        {
            throw new ImageOptimizationException(result.Error.Message);
        }

        return result.SecureUrl;
    }

    public async Task DeleteFilesAsync(string directoryName)
    {
        var result = await _cloudinary.DeleteResourcesByTagAsync(directoryName);
         
        if (result.Error is not null)
        {
            _logger.LogWarning("Failed to delete images // {details}", result.Error.Message);
        }
    }
}