namespace API.Endpoints.Courses.Stats;

public sealed class GetCourseStatsEndpoint(AppDbContext dbContext)
    : Endpoint<GetCourseStatsRequest, GetCourseStatsResponse>
{
    public override void Configure()
    {
        Get("courses/stats");
        Roles(Auth.Role.Manager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(GetCourseStatsRequest req, CancellationToken ct)
    {
        var query = dbContext.Courses.AsNoTracking()
            .Include(x => x.Creator)
            .Where(x => x.EndDate >= req.StartDate && x.EndDate <= req.EndDate);

        var recordsCount = await query.CountAsync(ct);

        var totalDuration = await query
                                .Select(x => x.DurationInHours)
                                .SumAsync(ct);

        var byCreator = await query
                            .GroupBy(x => new { x.CreatorId, x.Creator.Name })
                            .Select(g => new GetCourseStatsResponse.TeacherStatsResponse
                            {
                                Id = g.Key.CreatorId,
                                Name = g.Key.Name,
                                RecordsCount = g.Count(),
                                TotalDuration = g.Select(x => x.DurationInHours).Sum()
                            })
                            .OrderBy(x => x.Name)
                            .ThenBy(x => x.Id)
                            .ToListAsync(ct);

        await Send.OkAsync(new()
        {
            RecordsCount = recordsCount,
            TotalDuration = totalDuration,
            Teachers = byCreator
        }, ct);
    }
}