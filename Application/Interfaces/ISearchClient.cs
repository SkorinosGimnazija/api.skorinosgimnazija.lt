namespace Application.Interfaces
{
    using Application.Posts.Dtos;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface ISearchClient
    {
        Task SavePost(PostSearchDto post);
        Task RemovePost(int id);
        Task UpdatePost(PostSearchDto post);
        Task<List<PostSearchDto>> Search(string query);
    }
}
