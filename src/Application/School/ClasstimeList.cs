namespace SkorinosGimnazija.Application.School;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class ClasstimeList
{
    public record Query() : IRequest<List<ClasstimeDto>>;

    public class Handler : IRequestHandler<Query, List<ClasstimeDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ClasstimeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Classtimes
                       .AsNoTracking()
                       .OrderBy(x => x.Number)
                       .ProjectTo<ClasstimeDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}