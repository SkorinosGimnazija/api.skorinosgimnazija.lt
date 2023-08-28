namespace SkorinosGimnazija.Application.Timetable;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using SkorinosGimnazija.Application.Common.Interfaces;
using SkorinosGimnazija.Application.Events.Dtos;
using SkorinosGimnazija.Application.Timetable.Dtos;

public static class TimetablePublicList
{
    public record Query() : IRequest<List<TimetableDto>>;

    public class Handler : IRequestHandler<Query, List<TimetableDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public Handler(IAppDbContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<List<TimetableDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var day = (int)DateTime.Now.DayOfWeek;
            var key = "timetable" + day;

            if (!_cache.TryGetValue(key, out List<TimetableDto>? cached))
            {
                cached = await _context.Timetable
                             .AsNoTracking()
                             .ProjectTo<TimetableDto>(_mapper.ConfigurationProvider)
                             .Where(x => x.Day.Number == day)
                             .OrderBy(x => x.Room.Number)
                             .ToListAsync(cancellationToken);

                //todo invalidate cache on new record
                //_cache.Set(key, cached, TimeSpan.FromHours(1));
            }

            return cached!;
        }
    }
}