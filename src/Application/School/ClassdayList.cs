namespace SkorinosGimnazija.Application.School;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class ClassdayList
{
    public record Query() : IRequest<List<ClassdayDto>>;

    public class Handler : IRequestHandler<Query, List<ClassdayDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ClassdayDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Classdays
                       .AsNoTracking()
                       .OrderBy(x => x.Number)
                       .ProjectTo<ClassdayDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}