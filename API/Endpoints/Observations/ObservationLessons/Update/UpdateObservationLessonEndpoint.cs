namespace API.Endpoints.Observations.ObservationLessons.Update;

public sealed class UpdateObservationLessonEndpoint(AppDbContext dbContext)
    : Endpoint<UpdateObservationLessonRequest, ObservationLessonResponse, ObservationLessonMapper>
{
    public override void Configure()
    {
        Put("observations/lessons");
        Roles(Auth.Role.Admin);
        Description(x => x.WithTags(ObservationTags.Lessons));
    }

    public override async Task HandleAsync(UpdateObservationLessonRequest req, CancellationToken ct)
    {
        var entity = await dbContext.ObservationLessons.FindAsync([req.Id], ct);
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