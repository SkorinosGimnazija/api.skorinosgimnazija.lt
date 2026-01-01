namespace API.Endpoints.Observations.ObservationStudents.List;

using FastEndpoints.Swagger;

public sealed class ListObservationStudentsEndpoint(AppDbContext dbContext)
    : Endpoint<ListObservationStudentsRequest, IEnumerable<ObservationStudentResponse>,
        ObservationStudentMapper>
{
    public override void Configure()
    {
        Get("observations/students");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
        Description(x => x.AutoTagOverride(ObservationTags.Students));
    }

    public override async Task HandleAsync(ListObservationStudentsRequest req, CancellationToken ct)
    {
        var query = dbContext.ObservationStudents.AsNoTracking();

        if (req.ShowEnabledOnly is true)
        {
            query = query.Where(x => x.IsActive);
        }

        var entities = await query
                           .OrderBy(x => x.Name)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}