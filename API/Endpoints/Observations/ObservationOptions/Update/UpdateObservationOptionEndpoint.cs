namespace API.Endpoints.Observations.ObservationOptions.Update;

public sealed class UpdateObservationOptionEndpoint(AppDbContext dbContext)
    : Endpoint<UpdateObservationOptionRequest, ObservationOptionResponse, ObservationOptionMapper>
{
    public override void Configure()
    {
        Put("observations/options");
        Roles(Auth.Role.Admin);
        Description(x => x.WithTags(ObservationTags.Options));
    }

    public override async Task HandleAsync(UpdateObservationOptionRequest req, CancellationToken ct)
    {
        var entity = await dbContext.ObservationOptions.FindAsync([req.Id], ct);
        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        Map.UpdateEntity(req, entity);
        await dbContext.SaveChangesAsync(ct);

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}