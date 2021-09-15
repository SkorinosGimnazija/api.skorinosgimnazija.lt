namespace Infrastructure.ImageOptimization
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.Interfaces;
    using Microsoft.AspNetCore.Http;

    public interface IImageOptimizer
    {
        Task<OptimizedImage> OptimizeAsync(IFormFile image);

        Task DeleteAsync(string imageId);
    }
}
