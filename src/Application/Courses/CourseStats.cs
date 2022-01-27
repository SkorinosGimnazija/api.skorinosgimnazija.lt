﻿namespace SkorinosGimnazija.Application.Courses;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Identity;
using Common.Interfaces;
using Domain.Entities.Identity;
using Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
 
public static class CourseStats
{
    public record Query(DateTime Start, DateTime End) : IRequest<List<CourseStatsDto>>;
     
    public class Handler : IRequestHandler<Query, List<CourseStatsDto>>
    {
        private readonly IAppDbContext _context;

        public Handler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CourseStatsDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var start = DateOnly.FromDateTime(request.Start);
            var end = DateOnly.FromDateTime(request.End);

                var usersQuery = _context.Users
                    .AsNoTracking()
                    .Select(x => new
                    {
                        x.Id,
                        DisplayName = x.DisplayName ?? x.Email
                    })
                    .OrderBy(x => x.DisplayName);

                var coursesQuery = _context.Courses
                    .AsNoTracking()
                    .Where(x => x.EndDate >= start && x.EndDate <= end)
                    .GroupBy(x => x.UserId)
                    .Select(x => new
                    {
                        UserId = x.Key,
                        Hours = x.Sum(z => z.DurationInHours),
                        Price = x.Sum(z => z.Price),
                        Count = x.Count(),
                        UsefulCount = x.Count(z=> z.IsUseful),
                        LastUpdate = x.Max(z => z.CreatedAt)
                    });

                return await usersQuery
                           .AsNoTracking()
                           .Join(coursesQuery,
                               x => x.Id,
                               x => x.UserId,
                               (user, courses) => new CourseStatsDto
                               {
                                   UserId = user.Id,
                                   UserDisplayName = user.DisplayName,
                                   Price = courses.Price  ??0 ,
                                   Count = courses.Count,
                                   UsefulCount = courses.UsefulCount,
                                   LastUpdate = courses.LastUpdate,
                                   Hours = courses.Hours
                               })
                           .ToListAsync(cancellationToken);
        }
    }
}