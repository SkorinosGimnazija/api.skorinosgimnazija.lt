namespace API.Endpoints.Courses.List;

using API.Extensions;

public sealed class ListCoursesEndpoint(AppDbContext dbContext)
    : Endpoint<ListCoursesRequest, PaginatedListResponse<CourseResponse>, CourseMapper>
{
    public override void Configure()
    {
        Get("courses");
        Roles(Auth.Role.Teacher);
    }

    public override async Task HandleAsync(ListCoursesRequest req, CancellationToken ct)
    {
        if (!HasAccess(req))
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        var query = dbContext.Courses.AsNoTracking()
            .Include(x => x.Creator)
            .AsQueryable();

        if (req.UserId is not null)
        {
            query = query.Where(x => x.CreatorId == req.UserId)
                .OrderByDescending(x => x.Id);
        }
        else
        {
            query = query
                .OrderByDescending(x => x.StartDate)
                .ThenByDescending(x => x.Id);
        }

        var entities = await query
                           .Select(x => Map.FromEntity(x))
                           .ToPaginatedListAsync(req, ct);

        await Send.OkAsync(entities, ct);
    }

    private bool HasAccess(ListCoursesRequest req)
    {
        var canAccessRequested = User.HasId(req.UserId);
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