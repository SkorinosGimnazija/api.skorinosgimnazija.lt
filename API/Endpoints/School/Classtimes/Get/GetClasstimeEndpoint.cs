namespace API.Endpoints.School.Classtimes.Get;

public sealed class GetClasstimeEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, ClasstimeResponse, ClasstimeMapper>
{
    public override void Configure()
    {
        Get("school/classtimes/{id}");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Classtimes.FindAsync([req.Id], ct);
        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}