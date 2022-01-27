namespace SkorinosGimnazija.Application.Courses;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Identity;
using Common.Interfaces;
using Domain.Entities.Identity;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class CourseAdminList
{
    public record Query(int UserId, DateTime Start, DateTime End) : IRequest<List<CourseDto>>;
     
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
            var start = DateOnly.FromDateTime(request.Start);
            var end = DateOnly.FromDateTime(request.End);

            var coursesQuery = _context.Courses
                .AsNoTracking()
                .Where(x => x.EndDate >= start && x.EndDate <= end);

            if (request.UserId != 0)
            {
                coursesQuery = coursesQuery.Where(x => x.UserId == request.UserId);
            }

            return await coursesQuery
                       .OrderByDescending(x => x.EndDate)
                       .ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}