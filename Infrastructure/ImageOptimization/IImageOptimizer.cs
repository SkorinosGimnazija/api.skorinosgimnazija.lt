namespace Infrastructure.ImageOptimization
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public interface IImageOptimizer
    {
        Task<OptimizedImage> OptimizeAsync(IFormFile image);

        Task DeleteAsync(string imageId);
    }
}