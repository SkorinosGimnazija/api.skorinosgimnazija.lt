namespace SkorinosGimnazija.Infrastructure.ImageOptimization;

using Microsoft.AspNetCore.Http;

public interface IImageOptimizer
{
    Task<Uri> OptimizeAsync(IFormFile image, string directoryName, bool featuredImage);

    Task DeleteFilesAsync(string directoryName);
}