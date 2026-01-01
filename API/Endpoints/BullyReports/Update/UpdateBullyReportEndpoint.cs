namespace API.Endpoints.BullyReports.Update;

using API.Database.Entities.BullyReports;
using API.Extensions;

public sealed class UpdateBullyReportEndpoint(AppDbContext dbContext)
    : Endpoint<UpdateBullyReportRequest, BullyReportResponse, BullyReportMapper>
{
    public override void Configure()
    {
        Put("bully-reports");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(UpdateBullyReportRequest req, CancellationToken ct)
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

        Map.UpdateEntity(req, entity);
        await dbContext.SaveChangesAsync(ct);
        await dbContext.Entry(entity).Reference(x => x.Creator).LoadAsync(ct);

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