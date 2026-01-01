namespace API.Endpoints.School.Timetables.Stats;

using API.Extensions;

public sealed class GetTimetableStatsEndpoint(AppDbContext dbContext, TimeProvider timeProvider)
    : EndpointWithoutRequest<IEnumerable<TimetableStatsResponse>>
{
    public override void Configure()
    {
        Get("school/timetable");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var ltNow = timeProvider.LtNow;
        var today = DateOnly.FromDateTime(ltNow);

        var counts = await dbContext.Timetable
                         .AsNoTracking()
                         .GroupBy(x => new { x.RoomId, x.DayId })
                         .Select(g => new { g.Key.RoomId, g.Key.DayId, Count = g.Count() })
                         .ToListAsync(ct);

        var overrideDatesRaw = await dbContext.TimetableOverrides
                                   .AsNoTracking()
                                   .Where(x => x.Date >= today)
                                   .GroupBy(x => new { x.RoomId, x.Date })
                                   .Select(g => new { g.Key.RoomId, g.Key.Date })
                                   .ToListAsync(ct);

        var overridesByRoom = overrideDatesRaw
            .GroupBy(x => x.RoomId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Date).Distinct().OrderBy(d => d).ToList());

        var classrooms = await dbContext.Classrooms
                             .AsNoTracking()
                             .ToDictionaryAsync(x => x.Id, ct);

        var response = counts
            .GroupBy(x => x.RoomId)
            .Select(g => new TimetableStatsResponse
            {
                RoomId = g.Key,
                RoomName = classrooms[g.Key].Name,
                CountsByDay = g.ToDictionary(k => k.DayId, v => v.Count),
                OverrideDates = overridesByRoom.GetValueOrDefault(g.Key, [])
            })
            .OrderBy(x => x.RoomId)
            .ToList();

        await Send.OkAsync(response, ct);
    }
}