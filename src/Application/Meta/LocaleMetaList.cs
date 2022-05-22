namespace SkorinosGimnazija.Application.Meta;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public static class LocaleMetaList
{
    public record Query() : IRequest<List<LocaleMetaDto>>;

    public class Handler : IRequestHandler<Query, List<LocaleMetaDto>>
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

        public async Task<List<LocaleMetaDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            const string Key = "MetaLocales";

            if (!_cache.TryGetValue(Key, out List<LocaleMetaDto> cachedLocales))
            {
                var menuPostsQuery = _context.Menus
                    .AsNoTracking()
                    .Select(x => x.LinkedPostId);

                cachedLocales = await _context.Posts.AsNoTracking()
                                    .Where(x =>
                                        x.IsPublished &&
                                        x.ShowInFeed &&
                                        x.PublishedAt <= DateTime.UtcNow &&
                                        !menuPostsQuery.Contains(x.Id))
                                    .GroupBy(x => x.Language.Slug)
                                    .ProjectTo<LocaleMetaDto>(_mapper.ConfigurationProvider)
                                    .ToListAsync(cancellationToken);

                _cache.Set(Key, cachedLocales, TimeSpan.FromDays(1));
            }

            return cachedLocales;
        }
    }
}