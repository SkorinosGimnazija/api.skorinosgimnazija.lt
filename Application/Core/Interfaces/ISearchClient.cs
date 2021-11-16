namespace Application.Core.Interfaces;

using Posts.Dtos;

public interface ISearchClient
{
    Task SavePost(PostIndexDto post);

    Task RemovePost(int id);

    Task<List<int>> SearchPost(string query, CancellationToken ct);
}