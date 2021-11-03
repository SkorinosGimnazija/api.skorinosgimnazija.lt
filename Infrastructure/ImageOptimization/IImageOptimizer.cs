namespace Infrastructure.ImageOptimization;

using Microsoft.AspNetCore.Http;

public interface IImageOptimizer
{
    Task<OptimizedImage> OptimizeAsync(IFormFile image);

    Task DeleteAsync(string imageId);
}