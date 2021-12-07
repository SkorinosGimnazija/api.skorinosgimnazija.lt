namespace SkorinosGimnazija.Application.Common.Interfaces;

using Domain.Entities;
using Menus.Dtos;
using Pagination;
using Posts.Dtos;

public interface ISearchClient
{
    Task<PaginatedList<int>> SearchMenuAsync(string query, PaginationDto pagination, CancellationToken ct);

    Task<PaginatedList<int>> SearchPostAsync(string query, PaginationDto pagination, CancellationToken ct);

    Task SaveMenuAsync(MenuIndexDto post);

    Task RemoveMenuAsync(Menu menu);

    Task SavePostAsync(PostIndexDto post);

    Task RemovePostAsync(Post post);
}