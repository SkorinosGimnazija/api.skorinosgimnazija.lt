namespace SkorinosGimnazija.Application.Courses;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Interfaces;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class CourseAdminList
{
    public record Query(DateOnly Start, DateOnly End) : IRequest<List<CourseDto>>;

    public class Handler : IRequestHandler<Query, List<CourseDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CourseDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await _context.Courses
                       .AsNoTracking()
                       .Where(x => x.StartDate >= request.Start && x.EndDate <= request.End)
                       .ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}