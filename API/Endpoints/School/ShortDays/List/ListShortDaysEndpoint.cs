namespace API.Endpoints.School.ShortDays.List;

public sealed class ListShortDaysEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<IEnumerable<ShortDayResponse>, ShortDayMapper>
{
    public override void Configure()
    {
        Get("school/short-days");
        Roles(Auth.Role.Admin, Auth.Role.Manager);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await dbContext.ShortDays
                           .AsNoTracking()
                           .OrderByDescending(x => x.Id)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}