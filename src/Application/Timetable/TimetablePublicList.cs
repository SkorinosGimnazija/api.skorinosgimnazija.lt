namespace SkorinosGimnazija.Application.Timetable;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using School.Dtos;

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
            var datetime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "FLE Standard Time");
            var time = TimeOnly.FromDateTime(datetime);
            var date = DateOnly.FromDateTime(datetime);

            var isShortTime = await _context.ClasstimeShortDays
                                  .AsNoTracking()
                                  .AnyAsync(x => x.Date == date, cancellationToken);

            var classTime = await _context.Classtimes
                                .AsNoTracking()
                                .OrderBy(x => x.Number)
                                .Select(x => new ClasstimeDto
                                {
                                    Id = x.Id,
                                    Number = x.Number,
                                    StartTime = isShortTime ? x.StartTimeShort : x.StartTime,
                                    EndTime = isShortTime ? x.EndTimeShort : x.EndTime
                                })
                                .FirstOrDefaultAsync(x => x.EndTime > time, cancellationToken);

            if (classTime is null)
            {
                return null;
            }

            var day = (int) date.DayOfWeek;
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