namespace API.Endpoints.BullyReports.Delete;

using API.Database.Entities.BullyReports;
using API.Extensions;

public sealed class DeleteBullyReportEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest>
{
    public override void Configure()
    {
        Delete("bully-reports/{id}");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.BullyReports.FindAsync([req.Id], ct);

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

        dbContext.BullyReports.Remove(entity);
        await dbContext.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }

    private bool HasAccess(BullyReport entity)
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