namespace API.Endpoints.Achievements.Delete;

using API.Database.Entities.Achievements;
using API.Extensions;

public sealed class DeleteAchievementEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest>
{
    public override void Configure()
    {
        Delete("achievements/{id}");
        Roles(Auth.Role.Teacher);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Achievements.FindAsync([req.Id], ct);
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

        dbContext.Achievements.Remove(entity);
        await dbContext.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
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