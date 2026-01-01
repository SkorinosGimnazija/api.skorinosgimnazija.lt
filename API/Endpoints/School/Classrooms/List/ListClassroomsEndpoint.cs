namespace API.Endpoints.School.Classrooms.List;

public sealed class ListClassroomsEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<IEnumerable<ClassroomResponse>, ClassroomMapper>
{
    public override void Configure()
    {
        Get("school/classrooms");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await dbContext.Classrooms
                           .AsNoTracking()
                           .OrderBy(x => x.Id)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}