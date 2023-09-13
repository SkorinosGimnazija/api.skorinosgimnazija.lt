namespace SkorinosGimnazija.Application.School;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkorinosGimnazija.Application.Common.Interfaces;
using SkorinosGimnazija.Application.School.Dtos;

public static class AnnouncementsPublicList
{
    public record Query() : IRequest<List<AnnouncementDto>>;

    public class Handler : IRequestHandler<Query, List<AnnouncementDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AnnouncementDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var date = DateOnly.FromDateTime(DateTime.UtcNow);

            return await _context.Announcements
                       .AsNoTracking()
                       .Where(x => date >= x.StartTime && date <= x.EndTime)
                       .OrderBy(x => x.StartTime)
                       .ProjectTo<AnnouncementDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}