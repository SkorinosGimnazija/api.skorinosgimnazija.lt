namespace API.Endpoints.Observations.Entries.Create;

using API.Endpoints.Observations.Entries.Get;

public sealed class CreateObservationEndpoint(AppDbContext dbContext)
    : Endpoint<CreateObservationRequest, ObservationResponse, ObservationMapper>
{
    public override void Configure()
    {
        Post("observations");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(CreateObservationRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);
        entity.Options = await dbContext.ObservationOptions
                             .Where(x => req.OptionIds.Contains(x.Id))
                             .ToListAsync(ct);

        dbContext.Observations.Add(entity);
        await dbContext.SaveChangesAsync(ct);

        entity = await dbContext.Observations.AsNoTracking()
                     .Include(x => x.Lesson)
                     .Include(x => x.Student)
                     .Include(x => x.Creator)
                     .Include(x => x.Options)
                     .FirstAsync(x => x.Id == entity.Id, ct);

        await Send.CreatedAtAsync<GetObservationEndpoint>(
            new { id = entity.Id },
            Map.FromEntity(entity),
            cancellation: ct);
    }
}