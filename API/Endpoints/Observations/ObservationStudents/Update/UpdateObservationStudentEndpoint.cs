namespace API.Endpoints.Observations.ObservationStudents.Update;

public sealed class UpdateObservationStudentEndpoint(AppDbContext dbContext)
    : Endpoint<UpdateObservationStudentRequest, ObservationStudentResponse,
        ObservationStudentMapper>
{
    public override void Configure()
    {
        Put("observations/students");
        Roles(Auth.Role.Admin);
        Description(x => x.WithTags(ObservationTags.Students));
    }

    public override async Task HandleAsync(
        UpdateObservationStudentRequest req, CancellationToken ct)
    {
        var entity = await dbContext.ObservationStudents.FindAsync([req.Id], ct);

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