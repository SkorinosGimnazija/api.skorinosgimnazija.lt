namespace API.Endpoints.Observations.ObservationLessons.Get;

using FastEndpoints.Swagger;

public sealed class GetObservationLessonEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, ObservationLessonResponse, ObservationLessonMapper>
{
    public override void Configure()
    {
        Get("observations/lessons/{id}");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
        Description(x => x.AutoTagOverride(ObservationTags.Lessons));
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.ObservationLessons.FindAsync([req.Id], ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}