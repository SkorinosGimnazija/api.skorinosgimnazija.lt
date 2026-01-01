namespace API.Endpoints.Courses.Create;

using API.Endpoints.Courses.Get;

public sealed class CreateCourseEndpoint(AppDbContext dbContext)
    : Endpoint<CreateCourseRequest, CourseResponse, CourseMapper>
{
    public override void Configure()
    {
        Post("courses");
        Roles(Auth.Role.Teacher);
    }

    public override async Task HandleAsync(CreateCourseRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);

        dbContext.Courses.Add(entity);
        await dbContext.SaveChangesAsync(ct);

        await dbContext.Entry(entity).Reference(x => x.Creator).LoadAsync(ct);

        await Send.CreatedAtAsync<GetCourseEndpoint>(
            new { id = entity.Id },
            Map.FromEntity(entity),
            cancellation: ct);
    }
}