namespace API.Endpoints.Observations.ObservationOptions.List;

using FastEndpoints.Swagger;

public sealed class ListObservationOptionsEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<IEnumerable<ObservationOptionResponse>, ObservationOptionMapper>
{
    public override void Configure()
    {
        Get("observations/options");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
        Description(x => x.AutoTagOverride(ObservationTags.Options));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await dbContext.ObservationOptions
                           .AsNoTracking()
                           .OrderBy(x => x.Name)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}