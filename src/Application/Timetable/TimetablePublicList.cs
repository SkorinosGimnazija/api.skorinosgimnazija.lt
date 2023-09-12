namespace SkorinosGimnazija.Application.Timetable;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using School.Dtos;
using SkorinosGimnazija.Application.Common.Interfaces;
using SkorinosGimnazija.Application.Events.Dtos;
using SkorinosGimnazija.Application.Timetable.Dtos;

public static class TimetablePublicList
{
    public record Query() : IRequest<TimetablePublicDto?>;

    public class Handler : IRequestHandler<Query, TimetablePublicDto?>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TimetablePublicDto?> Handle(Query request, CancellationToken cancellationToken)
        {
            //todo change class times to utc...
            var time = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(3));

            var classTime = await _context.Classtimes
                                .AsNoTracking()
                                .OrderBy(x => x.Number)
                                .FirstOrDefaultAsync(x => x.EndTime > time, cancellationToken);

            if (classTime is null)
            {
                return null;
            }

            var day = (int)DateTime.Now.DayOfWeek;
            var timetable = await _context.Timetable
                             .AsNoTracking()
                             .Where(x => x.Day.Number == day && x.Time.Number == classTime.Number)
                             .OrderBy(x => x.Room.Number)
                             .ProjectTo<TimetableSimpleDto>(_mapper.ConfigurationProvider)
                             .ToListAsync(cancellationToken);

            return new()
            {
                Timetable = timetable,
                Classtime = _mapper.Map<ClasstimeSimpleDto>(classTime),
                CurrentTime = time.ToShortTimeString()
            };
        }
    }
}