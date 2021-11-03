namespace Application.Interfaces;

using Posts.Dtos;

public interface ISearchClient
{
    Task SavePost(PostSearchDto post);

    Task RemovePost(int id);

    Task UpdatePost(PostSearchDto post);

    Task<List<PostSearchDto>> Search(string query);
}