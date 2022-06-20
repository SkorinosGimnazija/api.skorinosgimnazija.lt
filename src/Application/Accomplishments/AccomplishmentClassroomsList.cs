namespace SkorinosGimnazija.Application.Accomplishments;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class AccomplishmentClassroomsList
{
    public record Query() : IRequest<List<AccomplishmentClassroomDto>>;

    public class Handler : IRequestHandler<Query, List<AccomplishmentClassroomDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<AccomplishmentClassroomDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.AccomplishmentClassrooms
                       .AsNoTracking()
                       .ProjectTo<AccomplishmentClassroomDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}