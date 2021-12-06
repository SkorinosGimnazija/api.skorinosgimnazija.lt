namespace SkorinosGimnazija.Application.Common.Interfaces;

using Pagination;
using Posts.Dtos;

public interface ISearchClient
{
    Task SavePostAsync(PostIndexDto post);

    Task RemovePostAsync(int id);

    Task<PaginatedList<int>> SearchPostAsync(string query, PaginationDto pagination, CancellationToken ct);
}