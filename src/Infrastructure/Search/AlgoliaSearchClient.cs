namespace SkorinosGimnazija.Infrastructure.Search;

using Algolia.Search.Clients;
using Application.Common.Exceptions;
using Application.Common.Pagination;
using Application.Posts.Dtos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Options;
using ISearchClient = Application.Common.Interfaces.ISearchClient;

public sealed class AlgoliaSearchClient : ISearchClient
{
    private readonly SearchIndex _menusIndex;
    private readonly SearchIndex _postsIndex;

    public AlgoliaSearchClient(IOptions<AlgoliaOptions> options, IHostEnvironment env)
    {
        var client = new SearchClient(options.Value.AppId, options.Value.ApiKey);
        var prefix = env.IsDevelopment() ? "dev_" : "prod_";

        _postsIndex = client.InitIndex(prefix + "posts");
        _menusIndex = client.InitIndex(prefix + "menus");
    }

    public async Task<PaginatedList<int>> SearchPostAsync(string query, PaginationDto pagination, CancellationToken ct)
    {
        try
        {
            var result = await _postsIndex.SearchAsync<PostIndexDto>(
                             new(query)
                             {
                                 HitsPerPage = pagination.Items,
                                 Page = pagination.Page
                             },
                             null,
                             ct);

            return new(
                result.Hits.ConvertAll(x => int.Parse(x.ObjectID)),
                result.NbHits,
                result.Page,
                result.HitsPerPage);
        }
        catch (Exception e)
        {
            throw new SearchIndexException("Search failed", e);
        }
    }

    public async Task SavePostAsync(PostIndexDto post)
    {
        try
        {
            //await _postsIndex.SaveObjectAsync(post);
        }
        catch (Exception e)
        {
            throw new SearchIndexException("Saving failed", e);
        }
    }

    public async Task RemovePostAsync(int id)
    {
        try
        {
            //await _postsIndex.DeleteObjectAsync(id.ToString());
        }
        catch (Exception e)
        {
            throw new SearchIndexException("Removing failed", e);
        }
    }
}