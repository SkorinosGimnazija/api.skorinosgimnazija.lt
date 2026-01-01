namespace API.Endpoints.School.Timetables.Delete;

public sealed class DeleteTimetableEndpoint(AppDbContext dbContext)
    : Endpoint<List<int>>
{
    public override void Configure()
    {
        Delete("school/timetable");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(List<int> req, CancellationToken ct)
    {
        await dbContext.Timetable
            .Where(x => req.Contains(x.DayId))
            .ExecuteDeleteAsync(ct);

        if (req.Contains(-1))
        {
            await dbContext.TimetableOverrides.ExecuteDeleteAsync(ct);
        }

        await Send.NoContentAsync(ct);
    }
}