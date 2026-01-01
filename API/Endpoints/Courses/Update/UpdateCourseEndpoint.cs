namespace API.Endpoints.Courses.Update;

using API.Database.Entities.Courses;
using API.Extensions;

public sealed class UpdateCourseEndpoint(AppDbContext dbContext)
    : Endpoint<UpdateCourseRequest, CourseResponse, CourseMapper>
{
    public override void Configure()
    {
        Put("courses");
        Roles(Auth.Role.Teacher);
    }

    public override async Task HandleAsync(UpdateCourseRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Courses
                         .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        if (!HasAccess(entity))
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        Map.UpdateEntity(req, entity);
        await dbContext.SaveChangesAsync(ct);

        await dbContext.Entry(entity).Reference(x => x.Creator).LoadAsync(ct);

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }

    private bool HasAccess(Course entity)
    {
        var canAccessRequested = User.HasId(entity.CreatorId);
        if (canAccessRequested)
        {
            return true;
        }

        var canAccessAny = User.IsAdmin();
        if (canAccessAny)
        {
            return true;
        }

        return false;
    }
}