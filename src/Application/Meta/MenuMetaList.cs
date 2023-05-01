namespace SkorinosGimnazija.Application.Meta;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

public static class MenuMetaList
{
    public record Query() : IRequest<List<MenuMetaDto>>;

    public class Handler : IRequestHandler<Query, List<MenuMetaDto>>
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

        public async Task<List<MenuMetaDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            const string Key = "MetaMenus";

            if (!_cache.TryGetValue(Key, out List<MenuMetaDto>? cachedMenus))
            {
                cachedMenus = await _context.Menus
                                  .AsNoTracking()
                                  .Where(x =>
                                      x.IsPublished &&
                                      x.LinkedPostId != null &&
                                      x.MenuLocation.Slug != "off")
                                  .ProjectTo<MenuMetaDto>(_mapper.ConfigurationProvider)
                                  .ToListAsync(cancellationToken);

                _cache.Set(Key, cachedMenus, TimeSpan.FromDays(1));
            }

            return cachedMenus!;
        }
    }
}