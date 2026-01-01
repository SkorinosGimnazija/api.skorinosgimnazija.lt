namespace API.Endpoints.Observations.ObservationLessons.List;

using FastEndpoints.Swagger;

public sealed class ListObservationLessonsEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<IEnumerable<ObservationLessonResponse>, ObservationLessonMapper>
{
    public override void Configure()
    {
        Get("observations/lessons");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
        Description(x => x.AutoTagOverride(ObservationTags.Lessons));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await dbContext.ObservationLessons
                           .AsNoTracking()
                           .OrderBy(x => x.Name)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}