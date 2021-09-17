using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    using Microsoft.AspNetCore.Http;

    public interface IFileManager
    {
        Task<List<string>> SaveImagesAsync(int id, IEnumerable<IFormFile> files);



        Task DeleteFilesAsync(int postId, List<string> files);

        Task DeleteAllFilesAsync(int postId);

        Task<List<string>> SaveFilesAsync(int postId, List<IFormFile> files);
    }
}
