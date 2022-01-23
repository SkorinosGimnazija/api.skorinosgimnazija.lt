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
    public record Query(DateTime Start, DateTime End) : IRequest<List<CourseDto>>;

    public class Handler : IRequestHandler<Query, List<CourseDto>>
    {
        private readonly IAppDbContext _context;

        private readonly IMapper _mapper;

        public Handler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        class Me
        {
            public string Name { get; init; } = default!;
            public float Hours { get; init; }
            public float? Price { get; init; }
            public DateTime LastUpdate { get; init; }
        }

        public async Task<List<CourseDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var start = DateOnly.FromDateTime(request.Start);
            var end = DateOnly.FromDateTime(request.End);
             
            //var gr = await _context.Courses
            //             .AsNoTracking()
            //             .Where(x => x.EndDate >= start && x.EndDate <= end)
            //             .GroupBy(_ => 0)
            //             .Select(x => new CourseStatsDto
            //             {
            //                 UserDisplayName = "Visi",
            //                 Hours = x.Sum(z => z.DurationInHours),
            //                 Price = x.Sum(z => z.Price),
            //                 LastUpdate = x.Max(z => z.CreatedAt)
            //             })
            //             .ToListAsync(cancellationToken);

            var users = _context.Users
                .AsNoTracking()
                .Select(x => new
                {
                    x.Id,
                    DisplayName = x.DisplayName ?? x.Email
                })
                .OrderBy(x => x.DisplayName);

            var courses = _context.Courses
                .AsNoTracking()
                .Where(x => x.EndDate >= start && x.EndDate <= end)
                .GroupBy(x => x.UserId)
                .Select(x => new
                {
                    UserId = x.Key,
                    Hours = x.Sum(z => z.DurationInHours),
                    Price = x.Sum(z => z.Price),
                    LastUpdate = x.Max(z => z.CreatedAt)
                });

            var merged = await users.AsNoTracking()
                             .Join(courses,
                                 x => x.Id,
                                 x => x.UserId,
                                 (user, dto) => new Me
                                 {
                                     Name = user.DisplayName,
                                     Price = dto.Price,
                                     LastUpdate = dto.LastUpdate,
                                     Hours = dto.Hours
                                 })
                             .ToListAsync(cancellationToken);



            return new List<CourseDto>();


            return await _context.Courses
                       .AsNoTracking()
                       .Where(x => x.EndDate >= start && x.EndDate <= end)
                       .OrderByDescending(x=> x.EndDate)
                       .ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
                       .ToListAsync(cancellationToken);
        }
    }
}