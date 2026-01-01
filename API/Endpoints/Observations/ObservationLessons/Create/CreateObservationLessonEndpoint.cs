namespace API.Endpoints.Observations.ObservationLessons.Create;

using API.Endpoints.Observations.ObservationLessons.Get;

public sealed class CreateObservationLessonEndpoint(AppDbContext dbContext)
    : Endpoint<CreateObservationLessonRequest, ObservationLessonResponse, ObservationLessonMapper>
{
    public override void Configure()
    {
        Post("observations/lessons");
        Roles(Auth.Role.Admin);
        Description(x => x.WithTags(ObservationTags.Lessons));
    }

    public override async Task HandleAsync(CreateObservationLessonRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);

        dbContext.ObservationLessons.Add(entity);
        await dbContext.SaveChangesAsync(ct);

        await Send.CreatedAtAsync<GetObservationLessonEndpoint>(
            new { id = entity.Id },
            Map.FromEntity(entity),
            cancellation: ct);
    }
}