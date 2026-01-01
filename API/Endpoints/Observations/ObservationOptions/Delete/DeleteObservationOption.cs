namespace API.Endpoints.Observations.ObservationOptions.Delete;

public sealed class DeleteObservationOption(AppDbContext dbContext)
    : Endpoint<RouteIdRequest>
{
    public override void Configure()
    {
        Delete("observations/options/{id}");
        Roles(Auth.Role.Admin);
        Description(x => x.WithTags(ObservationTags.Options));
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.ObservationOptions.FindAsync([req.Id], ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        dbContext.ObservationOptions.Remove(entity);
        await dbContext.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}