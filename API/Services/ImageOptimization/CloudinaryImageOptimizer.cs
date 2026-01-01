namespace API.Services.ImageOptimization;

using API.Services.Options;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

public sealed class CloudinaryImageOptimizer(
    IOptions<CloudinaryOptions> cloudinaryOptions,
    IHostEnvironment env)
    : IImageOptimizer
{
    private readonly Cloudinary _cloudinary = new(cloudinaryOptions.Value.Url);

    private readonly Transformation _featuredImageTransform = new Transformation()
        .Quality("95")
        .Width("300")
        .Height("300")
        .Crop(env.IsDevelopment() ? "lfill" : "imagga_scale")
        .Effect(env.IsDevelopment() ? "improve" : "viesus_correct")
        .FetchFormat("jpg");

    private readonly Transformation _galleryTransform = new Transformation()
        .Quality("95")
        .AspectRatio("16:9")
        .Width("1920")
        .Crop(env.IsDevelopment() ? "fill" : "imagga_crop")
        .Effect(env.IsDevelopment() ? "improve" : "viesus_correct")
        .FetchFormat("jpg");

    public async Task<Uri> OptimizeAsync(IFormFile image, string tag, bool featuredImage)
    {
        await using var stream = image.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            Transformation = featuredImage ? _featuredImageTransform : _galleryTransform,
            File = new(image.FileName, stream),
            Tags = tag
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error is not null)
        {
            throw new(result.Error.Message);
        }

        return result.SecureUrl;
    }

    public async Task DeleteFilesByTagAsync(string tag)
    {
        var result = await _cloudinary.DeleteResourcesByTagAsync(tag);
        if (result.Error is not null)
        {
            throw new(result.Error.Message);
        }
    }
}