namespace SkorinosGimnazija.Application.Accomplishments;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AccomplishmentAdminList
{
    public record Query(DateTime Start, DateTime End) : IRequest<List<AccomplishmentDto>>;

    public class Handler : IRequestHandler<Query, List<AccomplishmentDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AccomplishmentDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var start = DateOnly.FromDateTime(request.Start);
            var end = DateOnly.FromDateTime(request.End);

            return await _context.Accomplishments
                       .AsNoTracking()
                       .Where(x =>
                           x.Date >= start &&
                           x.Date <= end)
                       .OrderByDescending(x => x.Date)
                       .ThenByDescending(x=> x.Id)
                       .ProjectTo<AccomplishmentDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}