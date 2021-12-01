namespace SkorinosGimnazija.Application.Common.Interfaces;

using Posts.Dtos;

public interface ISearchClient
{
    Task SavePostAsync(PostIndexDto post);

    Task RemovePostAsync(int id);

    Task<List<int>> SearchPostAsync(string query, CancellationToken ct);
}