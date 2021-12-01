namespace SkorinosGimnazija.Infrastructure.ImageOptimization;

using Microsoft.AspNetCore.Http;

public interface IImageOptimizer
{
    Task<Uri> OptimizeAsync(IFormFile image, string directoryName);

    Task DeleteFilesAsync(string directoryName);
}