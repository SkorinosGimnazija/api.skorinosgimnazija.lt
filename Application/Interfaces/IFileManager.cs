namespace Application.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public interface IFileManager
    {
        Task<List<string>> SaveImagesAsync(int id, IFormFileCollection files);

        Task DeleteFilesAsync( List<string> files);

        Task DeleteAllFilesAsync(int postId);

        Task<List<string>> SaveFilesAsync(int postId, IFormFileCollection files);
    }
}