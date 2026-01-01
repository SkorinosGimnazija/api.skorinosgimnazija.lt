namespace API.Endpoints.School.Classrooms.Get;

public sealed class GetClassroomEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, ClassroomResponse, ClassroomMapper>
{
    public override void Configure()
    {
        Get("school/classrooms/{id}");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Classrooms.FindAsync([req.Id], ct);
        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}