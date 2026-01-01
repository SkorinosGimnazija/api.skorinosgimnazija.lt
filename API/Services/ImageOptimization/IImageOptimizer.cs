namespace API.Services.ImageOptimization;

public interface IImageOptimizer
{
    Task<Uri> OptimizeAsync(IFormFile image, string tag, bool featuredImage);

    Task DeleteFilesByTagAsync(string tag);
}