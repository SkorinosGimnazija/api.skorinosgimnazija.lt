namespace SkorinosGimnazija.Application.Accomplishments;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AccomplishmentScalesList
{
    public record Query() : IRequest<List<AccomplishmentScaleDto>>;

    public class Handler : IRequestHandler<Query, List<AccomplishmentScaleDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AccomplishmentScaleDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.AccomplishmentScales
                       .AsNoTracking()
                       .ProjectTo<AccomplishmentScaleDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}