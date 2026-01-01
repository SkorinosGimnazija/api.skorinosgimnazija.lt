namespace API.Endpoints.Observations.ObservationLessons.Delete;

public sealed class DeleteObservationLessonEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest>
{
    public override void Configure()
    {
        Delete("observations/lessons/{id}");
        Roles(Auth.Role.Admin);
        Description(x => x.WithTags(ObservationTags.Lessons));
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.ObservationLessons.FindAsync([req.Id], ct);
        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        dbContext.ObservationLessons.Remove(entity);
        await dbContext.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}