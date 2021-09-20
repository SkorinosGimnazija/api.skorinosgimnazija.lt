namespace Infrastructure.Search
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Algolia.Search.Clients;
    using Application.Posts.Dtos;
    using Microsoft.Extensions.Options;
    using ISearchClient = Application.Interfaces.ISearchClient;

    public class AlgoliaSearchClient : ISearchClient
    {
        private readonly SearchClient _client;
        private readonly SearchIndex _postsIndex;

        public AlgoliaSearchClient(IOptions<AlgoliaSettings> options)
        {
            _client = new(options.Value.AppId, options.Value.ApiKey);
            _postsIndex = _client.InitIndex("posts");
        }

        public async Task<List<PostSearchDto>> Search(string query)
        {
            var result = await _postsIndex.SearchAsync<PostSearchDto>(new(query));
            return result.Hits;
        }

        public async Task SavePost(PostSearchDto post)
        {
            if (!post.IsPublished)
            {
                return;
            }

            await _postsIndex.SaveObjectAsync(post);
        }

        public async Task RemovePost(int id)
        {
            await _postsIndex.DeleteObjectAsync(id.ToString());
        }

        public async Task UpdatePost(PostSearchDto post)
        {
            await RemovePost(int.Parse(post.ObjectID));
            await SavePost(post);
        }
    }
}