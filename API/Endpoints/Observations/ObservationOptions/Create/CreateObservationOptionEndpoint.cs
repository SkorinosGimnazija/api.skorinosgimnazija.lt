namespace API.Endpoints.Observations.ObservationOptions.Create;

using API.Endpoints.Observations.ObservationOptions.Get;

public sealed class CreateObservationOptionEndpoint(AppDbContext dbContext)
    : Endpoint<CreateObservationOptionRequest, ObservationOptionResponse, ObservationOptionMapper>
{
    public override void Configure()
    {
        Post("observations/options");
        Roles(Auth.Role.Admin);
        Description(x => x.WithTags(ObservationTags.Options));
    }

    public override async Task HandleAsync(CreateObservationOptionRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);

        dbContext.ObservationOptions.Add(entity);
        await dbContext.SaveChangesAsync(ct);

        await Send.CreatedAtAsync<GetObservationOptionEndpoint>(
            new { id = entity.Id },
            Map.FromEntity(entity),
            cancellation: ct);
    }
}