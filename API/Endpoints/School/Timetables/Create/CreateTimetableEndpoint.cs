namespace API.Endpoints.School.Timetables.Create;

using API.Extensions;

public sealed class CreateTimetableEndpoint(AppDbContext dbContext, TimeProvider timeProvider)
    : Endpoint<List<CreateTimetableRequest>, EmptyResponse, TimetableMapper>
{
    public override void Configure()
    {
        Post("school/timetable");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(List<CreateTimetableRequest> req, CancellationToken ct)
    {
        foreach (var group in req.GroupBy(x => x.DayId))
        {
            var dayId = group.Key;
            var timetableExists = await dbContext.Timetable.AnyAsync(x => x.DayId == dayId, ct);

            if (timetableExists)
            {
                var date = GetClosestDate(dayId);
                var entities = group.Select(x => Map.ToOverrideEntity(x, date));

                dbContext.TimetableOverrides.AddRange(entities);
            }
            else
            {
                var entities = group.Select(Map.ToEntity);

                dbContext.Timetable.AddRange(entities);
            }

            await dbContext.SaveChangesAsync(ct);
        }

        await Send.StatusCodeAsync(StatusCodes.Status201Created, ct);
    }

    private DateOnly GetClosestDate(int targetDayOfWeek)
    {
        var ltNow = timeProvider.LtNow;
        var date = DateOnly.FromDateTime(ltNow);
        var currentDayOfWeek = (int) date.DayOfWeek;
        var daysToAdd = (targetDayOfWeek - currentDayOfWeek + 7) % 7;

        if (daysToAdd == 0 && ltNow.Hour > 10)
        {
            daysToAdd = 7;
        }

        return date.AddDays(daysToAdd);
    }
}