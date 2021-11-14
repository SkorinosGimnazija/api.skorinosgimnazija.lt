namespace Infrastructure.Search;

using Algolia.Search.Clients;
using Application.Posts.Dtos;
using Microsoft.Extensions.Options;
using ISearchClient = Application.Interfaces.ISearchClient;

public class AlgoliaSearchClient : ISearchClient
{
    private readonly SearchIndex _menusIndex;
    private readonly SearchIndex _postsIndex;

    public AlgoliaSearchClient(IOptions<AlgoliaSettings> options)
    {
        //TODO pagination ?

        var client = new SearchClient(options.Value.AppId, options.Value.ApiKey);

        _postsIndex = client.InitIndex(options.Value.IndexPrefix + "posts");
        _menusIndex = client.InitIndex(options.Value.IndexPrefix + "menus");
    }

    public async Task<List<int>> SearchPost(string query, CancellationToken ct)
    {
        var result = await _postsIndex.SearchAsync<PostIndexDto>(new(query), null, ct);
        var ids = result.Hits.ConvertAll(x => int.Parse(x.ObjectID));

        return ids;
    }

    public async Task SavePost(PostIndexDto post)
    {
        await _postsIndex.SaveObjectAsync(post);
    }

    public async Task RemovePost(int id)
    {
        await _postsIndex.DeleteObjectAsync(id.ToString());
    }
}