namespace API.Endpoints.Courses.Get;

using API.Database.Entities.Courses;
using API.Extensions;

public sealed class GetCourseEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, CourseResponse, CourseMapper>
{
    public override void Configure()
    {
        Get("courses/{id}");
        Roles(Auth.Role.Teacher);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Courses.AsNoTracking()
                         .Include(x => x.Creator)
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

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }

    private bool HasAccess(Course entity)
    {
        var canAccessRequested = User.HasId(entity.CreatorId);
        if (canAccessRequested)
        {
            return true;
        }

        var canAccessAny = User.IsAdmin() || User.IsManager();
        if (canAccessAny)
        {
            return true;
        }

        return false;
    }
}