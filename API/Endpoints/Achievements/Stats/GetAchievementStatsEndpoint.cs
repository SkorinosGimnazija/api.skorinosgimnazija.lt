namespace API.Endpoints.Achievements.Stats;

public sealed class GetAchievementStatsEndpoint(AppDbContext dbContext)
    : Endpoint<GetAchievementStatsRequest, GetAchievementStatsResponse>
{
    public override void Configure()
    {
        Get("achievements/stats");
        Roles(Auth.Role.Manager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(GetAchievementStatsRequest req, CancellationToken ct)
    {
        var query = dbContext.Achievements.AsNoTracking()
            .Include(x => x.AdditionalTeachers)
            .Include(x => x.Students)
            .ThenInclude(s => s.AchievementType)
            .Where(x => x.Date >= req.StartDate && x.Date <= req.EndDate);

        var recordsCount = await query.CountAsync(ct);

        var studentsCount = await query
                                .SelectMany(x => x.Students)
                                .CountAsync(ct);

        var additionalTeachers = await query
                                     .SelectMany(x => x.AdditionalTeachers)
                                     .CountAsync(ct);

        var totalTeachersCount = recordsCount + additionalTeachers;

        var countByType = await query
                              .SelectMany(x => x.Students)
                              .GroupBy(s => new { s.AchievementTypeId, s.AchievementType.Name })
                              .Select(g => new GetAchievementStatsResponse.TypeStatsResponse
                              {
                                  Id = g.Key.AchievementTypeId,
                                  Name = g.Key.Name,
                                  Count = g.Count()
                              })
                              .OrderBy(x => x.Id)
                              .ToListAsync(ct);

        await Send.OkAsync(new()
        {
            RecordsCount = recordsCount,
            StudentsCount = studentsCount,
            TeachersCount = totalTeachersCount,
            TypeStats = countByType
        }, ct);
    }
}