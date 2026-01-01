namespace API.Endpoints.Observations.ObservationStudents.Delete;

public sealed class DeleteObservationStudentEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest>
{
    public override void Configure()
    {
        Delete("observations/students/{id}");
        Roles(Auth.Role.Admin);
        Description(x => x.WithTags(ObservationTags.Students));
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.ObservationStudents.FindAsync([req.Id], ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        dbContext.ObservationStudents.Remove(entity);
        await dbContext.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}