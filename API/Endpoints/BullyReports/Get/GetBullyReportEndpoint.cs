namespace API.Endpoints.BullyReports.Get;

using API.Database.Entities.BullyReports;
using API.Extensions;

public sealed class GetBullyReportEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, BullyReportResponse, BullyReportMapper>
{
    public override void Configure()
    {
        Get("bully-reports/{id}");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.BullyReports.AsNoTracking()
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

    private bool HasAccess(BullyReport entity)
    {
        var canAccessRequested = User.HasId(entity.CreatorId);
        if (canAccessRequested)
        {
            return true;
        }

        var canAccessAny = User.IsAdmin() || User.IsSocialManager() || User.IsManager();
        if (canAccessAny)
        {
            return true;
        }

        return false;
    }
}