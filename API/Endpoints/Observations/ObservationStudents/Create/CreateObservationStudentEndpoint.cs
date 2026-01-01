namespace API.Endpoints.Observations.ObservationStudents.Create;

using API.Endpoints.Observations.ObservationStudents.Get;

public sealed class CreateObservationStudentEndpoint(AppDbContext dbContext)
    : Endpoint<CreateObservationStudentRequest, ObservationStudentResponse,
        ObservationStudentMapper>
{
    public override void Configure()
    {
        Post("observations/students");
        Roles(Auth.Role.Admin);
        Description(x => x.WithTags(ObservationTags.Students));
    }

    public override async Task HandleAsync(
        CreateObservationStudentRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);

        dbContext.ObservationStudents.Add(entity);
        await dbContext.SaveChangesAsync(ct);

        await Send.CreatedAtAsync<GetObservationStudentEndpoint>(
            new { id = entity.Id },
            Map.FromEntity(entity),
            cancellation: ct);
    }
}