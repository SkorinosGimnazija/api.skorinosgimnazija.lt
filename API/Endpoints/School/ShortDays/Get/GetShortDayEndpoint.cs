namespace API.Endpoints.School.ShortDays.Get;

public sealed class GetShortDayEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, ShortDayResponse, ShortDayMapper>
{
    public override void Configure()
    {
        Get("school/short-days/{id}");
        Roles(Auth.Role.Admin, Auth.Role.Manager);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.ShortDays.FindAsync([req.Id], ct);
        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}