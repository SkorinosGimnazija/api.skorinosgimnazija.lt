namespace SkorinosGimnazija.Application.Meta;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public static class PostMetaList
{
    public record Query() : IRequest<List<PostMetaDto>>;

    public class Handler : IRequestHandler<Query, List<PostMetaDto>>
    {
        private readonly IMemoryCache _cache;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<PostMetaDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            const string Key = "MetaPosts";

            if (!_cache.TryGetValue(Key, out List<PostMetaDto> cachedPosts))
            {
                var menuPostsQuery = _context.Menus
                    .Select(x => x.LinkedPostId);

                cachedPosts = await _context.Posts
                                  .AsNoTracking()
                                  .Where(x =>
                                      x.IsPublished &&
                                      x.PublishedAt <= DateTime.UtcNow &&
                                      !menuPostsQuery.Contains(x.Id))
                                  .ProjectTo<PostMetaDto>(_mapper.ConfigurationProvider)
                                  .ToListAsync(cancellationToken);

                _cache.Set(Key, cachedPosts, TimeSpan.FromDays(1));
            }

            return cachedPosts;
        }
    }
}