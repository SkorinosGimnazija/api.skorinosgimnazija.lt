namespace API.Endpoints.Observations.ObservationStudents.Get;

using FastEndpoints.Swagger;

public sealed class GetObservationStudentEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, ObservationStudentResponse, ObservationStudentMapper>
{
    public override void Configure()
    {
        Get("observations/students/{id}");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
        Description(x => x.AutoTagOverride(ObservationTags.Students));
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.ObservationStudents.FindAsync([req.Id], ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}