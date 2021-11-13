namespace Infrastructure.ImageOptimization;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

public class CloudinaryImageOptimizer : IImageOptimizer
{
    private readonly Cloudinary _cloudinary;
    private readonly Transformation _defaultTransformation;

    public CloudinaryImageOptimizer(IOptions<CloudinarySettings> options)
    {
        _cloudinary = new(options.Value.Url);
        _defaultTransformation = new Transformation()
            .Quality("90")
            .Gravity("face")
            .AspectRatio("16:9")
            .Width(1600)
            .Crop("fill")
            .Effect("improve")
            //.Effect("viesus_correct")
            .FetchFormat("jpg");
    }

    public async Task<OptimizedImage> OptimizeAsync(IFormFile image)
    {
        await using var stream = image.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            Transformation = _defaultTransformation, File = new(image.FileName, stream)
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.Error != null)
        {
            throw new(result.Error.Message);
        }

        return new(result.PublicId, result.SecureUrl);
    }

    public async Task DeleteAsync(string imageId)
    {
        await _cloudinary.DestroyAsync(new(imageId));
    }
}