namespace SkorinosGimnazija.Application.Common.Interfaces;

using Banners.Dtos;
using Domain.Entities.CMS;
using Menus.Dtos;
using Pagination;
using Posts.Dtos;

public interface ISearchClient
{
    Task<PaginatedList<int>> SearchBannersAsync(string query, PaginationDto pagination, CancellationToken ct);

    Task<PaginatedList<int>> SearchMenuAsync(string query, PaginationDto pagination, CancellationToken ct);

    Task<PaginatedList<int>> SearchPostAsync(string query, PaginationDto pagination, CancellationToken ct);

    Task SaveMenuAsync(MenuIndexDto menu);

    Task RemoveMenuAsync(Menu menu);

    Task SavePostAsync(PostIndexDto post);

    Task RemovePostAsync(Post post);

    Task SaveBannerAsync(BannerIndexDto banner);

    Task RemoveBannerAsync(Banner banner);
}