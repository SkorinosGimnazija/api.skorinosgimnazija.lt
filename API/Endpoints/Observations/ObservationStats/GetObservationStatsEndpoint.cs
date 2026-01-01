namespace API.Endpoints.Observations.ObservationStats;

public sealed class GetObservationStatsEndpoint(AppDbContext dbContext)
    : Endpoint<GetObservationStatsRequest, GetObservationStatsResponse>
{
    public override void Configure()
    {
        Get("observations/stats");
        Roles(Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(GetObservationStatsRequest req, CancellationToken ct)
    {
        var entities = await dbContext.Observations.AsNoTracking()
                           .Include(x => x.Creator)
                           .Include(x => x.Options)
                           .Where(x =>
                               x.StudentId == req.StudentId &&
                               x.Date >= req.StartDate &&
                               x.Date <= req.EndDate)
                           .OrderBy(x => x.Date)
                           .ThenBy(x => x.Id)
                           .ToListAsync(ct);

        var optionIds = await dbContext.ObservationOptions.AsNoTracking()
                            .Select(x => x.Id)
                            .ToListAsync(ct);

        var notes = entities
            .Where(x => !string.IsNullOrWhiteSpace(x.Note))
            .Select(x => new GetObservationStatsResponse.Note
            {
                Id = x.Id,
                CreatorName = x.Creator.Name,
                Text = x.Note!
            })
            .OrderBy(x => x.CreatorName)
            .ThenBy(x => x.Id)
            .ToList();

        var teacherRecords = entities
            .GroupBy(x => new { Id = x.Creator.UserName, x.Creator.Name })
            .Select(x => new GetObservationStatsResponse.TeacherRecordCount
            {
                Id = x.Key.Id,
                Name = x.Key.Name,
                Count = x.Count()
            })
            .OrderBy(x => x.Name)
            .ToList();

        var stats = entities
            .GroupBy(x => x.Date.ToString("yyyy-MM-01"))
            .ToDictionary(
                group => group.Key,
                group => optionIds
                    .ToDictionary(
                        id => id,
                        id => group.SelectMany(x => x.Options).Count(x => x.Id == id)
                    )
            );

        await Send.OkAsync(new()
        {
            Stats = stats,
            Notes = notes,
            TeacherRecordsCount = teacherRecords
        }, ct);
    }
}