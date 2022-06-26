namespace SkorinosGimnazija.Application.Accomplishments;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AccomplishmentAchievementsList
{
    public record Query() : IRequest<List<AccomplishmentAchievementDto>>;

    public class Handler : IRequestHandler<Query, List<AccomplishmentAchievementDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AccomplishmentAchievementDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.AccomplishmentAchievements
                       .AsNoTracking()
                       .ProjectTo<AccomplishmentAchievementDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}