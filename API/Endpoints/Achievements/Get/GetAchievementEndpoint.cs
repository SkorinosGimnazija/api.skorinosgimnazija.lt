namespace API.Endpoints.Achievements.Get;

using API.Database.Entities.Achievements;
using API.Extensions;

public sealed class GetAchievementEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, AchievementResponse, AchievementMapper>
{
    public override void Configure()
    {
        Get("achievements/{id}");
        Roles(Auth.Role.Teacher);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Achievements.AsNoTracking()
                         .Include(x => x.Creator)
                         .Include(x => x.AdditionalTeachers)
                         .Include(x => x.Scale)
                         .Include(x => x.Students)
                         .ThenInclude(s => s.Classroom)
                         .Include(x => x.Students)
                         .ThenInclude(s => s.AchievementType)
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

    private bool HasAccess(Achievement entity)
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