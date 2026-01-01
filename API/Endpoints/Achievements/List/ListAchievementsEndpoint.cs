namespace API.Endpoints.Achievements.List;

using API.Extensions;

public sealed class ListAchievementsEndpoint(AppDbContext dbContext)
    : Endpoint<ListAchievementsRequest, PaginatedListResponse<AchievementResponse>,
        AchievementMapper>
{
    public override void Configure()
    {
        Get("achievements");
        Roles(Auth.Role.Teacher);
    }

    public override async Task HandleAsync(ListAchievementsRequest req, CancellationToken ct)
    {
        if (!HasAccess(req))
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        var query = dbContext.Achievements.AsNoTracking()
            .Include(x => x.Creator)
            .Include(x => x.AdditionalTeachers)
            .Include(x => x.Scale)
            .Include(x => x.Students)
            .ThenInclude(s => s.Classroom)
            .Include(x => x.Students)
            .ThenInclude(s => s.AchievementType)
            .AsQueryable();

        if (req.UserId is not null)
        {
            query = query.Where(x =>
                    x.CreatorId == req.UserId ||
                    x.AdditionalTeachers.Any(u => u.Id == req.UserId))
                .OrderByDescending(x => x.Id);
        }
        else
        {
            query = query
                .OrderByDescending(x => x.Date)
                .ThenByDescending(x => x.Id);
        }

        var entities = await query
                           .Select(x => Map.FromEntity(x))
                           .ToPaginatedListAsync(req, ct);

        await Send.OkAsync(entities, ct);
    }

    private bool HasAccess(ListAchievementsRequest req)
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