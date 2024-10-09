namespace SkorinosGimnazija.Infrastructure.Search;

using Algolia.Search.Clients;
using Algolia.Search.Models.Search;
using Application.Banners.Dtos;
using Application.Common.Exceptions;
using Application.Common.Pagination;
using Application.Menus.Dtos;
using Application.Posts.Dtos;
using Domain.Entities.CMS;
using Domain.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Banner = Domain.Entities.CMS.Banner;
using ISearchClient = Application.Common.Interfaces.ISearchClient;

public sealed class AlgoliaSearchClient : ISearchClient
{
    private readonly string _bannersIndexName;
    private readonly SearchClient _client;
    private readonly string _menusIndexName;
    private readonly string _postsIndexName;

    public AlgoliaSearchClient(IOptions<AlgoliaOptions> options, IHostEnvironment env)
    {
        _client = new(options.Value.AppId, options.Value.ApiKey);
        var prefix = env.IsDevelopment() ? "dev_" : "prod_";

        _postsIndexName = prefix + "posts";
        _menusIndexName = prefix + "menus";
        _bannersIndexName = prefix + "banners";
    }

    public async Task<PaginatedList<int>> SearchBannersAsync(
        string query, PaginationDto pagination, CancellationToken ct)
    {
        try
        {
            return await SearchAsync(query, pagination, _bannersIndexName, ct);
        }
        catch (Exception e)
        {
            throw new SearchIndexException("Banner search failed", e);
        }
    }

    public async Task<PaginatedList<int>> SearchMenuAsync(string query, PaginationDto pagination, CancellationToken ct)
    {
        try
        {
            return await SearchAsync(query, pagination, _menusIndexName, ct);
        }
        catch (Exception e)
        {
            throw new SearchIndexException("Menu search failed", e);
        }
    }

    public async Task<PaginatedList<int>> SearchPostAsync(string query, PaginationDto pagination, CancellationToken ct)
    {
        try
        {
            return await SearchAsync(query, pagination, _postsIndexName, ct);
        }
        catch (Exception e)
        {
            throw new SearchIndexException("Post search failed", e);
        }
    }

    public async Task SaveMenuAsync(MenuIndexDto menu)
    {
        try
        {
            await _client.SaveObjectAsync(_menusIndexName, menu);
        }
        catch (Exception e)
        {
            throw new SearchIndexException("Menu saving failed", e);
        }
    }

    public async Task RemoveMenuAsync(Menu menu)
    {
        try
        {
            await _client.DeleteObjectAsync(_menusIndexName, menu.Id.ToString());
        }
        catch (Exception e)
        {
            throw new SearchIndexException("Menu removing failed", e);
        }
    }

    public async Task SavePostAsync(PostIndexDto post)
    {
        try
        {
            await _client.SaveObjectAsync(_postsIndexName, post);
        }
        catch (Exception e)
        {
            throw new SearchIndexException("Post saving failed", e);
        }
    }

    public async Task RemovePostAsync(Post post)
    {
        try
        {
            await _client.DeleteObjectAsync(_postsIndexName, post.Id.ToString());
        }
        catch (Exception e)
        {
            throw new SearchIndexException("Post Removing failed", e);
        }
    }

    public async Task SaveBannerAsync(BannerIndexDto banner)
    {
        try
        {
            await _client.SaveObjectAsync(_bannersIndexName, banner);
        }
        catch (Exception e)
        {
            throw new SearchIndexException("Banner saving failed", e);
        }
    }

    public async Task RemoveBannerAsync(Banner banner)
    {
        try
        {
            await _client.DeleteObjectAsync(_bannersIndexName, banner.Id.ToString());
        }
        catch (Exception e)
        {
            throw new SearchIndexException("Banner Removing failed", e);
        }
    }

    private async Task<PaginatedList<int>> SearchAsync(
        string query,
        PaginationDto pagination,
        string index,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new([], 0, pagination.Page, pagination.Items);
        }

        var result = await _client.SearchSingleIndexAsync<Hit>(index, new(
                             new SearchParamsObject
                             {
                                 Query = Uri.UnescapeDataString(query),
                                 HitsPerPage = pagination.Items,
                                 Page = pagination.Page
                             }),
                         null,
                         ct);

        return new(
            result.Hits.ConvertAll(x => int.Parse(x.ObjectID)),
            result.NbHits ?? 0,
            result.Page ?? 0,
            result.HitsPerPage ?? 0);
    }
}