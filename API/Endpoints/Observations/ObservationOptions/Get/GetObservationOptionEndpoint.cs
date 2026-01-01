namespace API.Endpoints.Observations.ObservationOptions.Get;

using FastEndpoints.Swagger;

public sealed class GetObservationOptionEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, ObservationOptionResponse, ObservationOptionMapper>
{
    public override void Configure()
    {
        Get("observations/options/{id}");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
        Description(x => x.AutoTagOverride(ObservationTags.Options));
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.ObservationOptions.FindAsync([req.Id], ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}