namespace SkorinosGimnazija.Application.School;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class ClassroomList
{
    public record Query() : IRequest<List<ClassroomDto>>;

    public class Handler : IRequestHandler<Query, List<ClassroomDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ClassroomDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Classrooms
                       .AsNoTracking()
                       .OrderBy(x => x.Number)
                       .ProjectTo<ClassroomDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}