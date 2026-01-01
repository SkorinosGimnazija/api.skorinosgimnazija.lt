namespace API.Endpoints.FailureReports.Update;

using API.Database.Entities.FailureReports;
using API.Extensions;

public sealed class UpdateFailureReportEndpoint(AppDbContext dbContext)
    : Endpoint<UpdateFailureReportRequest, FailureReportResponse, FailureReportMapper>
{
    public override void Configure()
    {
        Put("failure-reports");
    }

    public override async Task HandleAsync(UpdateFailureReportRequest req, CancellationToken ct)
    {
        var entity = await dbContext.FailureReports.FindAsync([req.Id], ct);
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

    private bool HasAccess(FailureReport entity)
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