namespace API.Endpoints.FailureReports.Delete;

using API.Database.Entities.FailureReports;
using API.Extensions;

public sealed class DeleteFailureReportEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest>
{
    public override void Configure()
    {
        Delete("failure-reports/{id}");
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
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

        dbContext.FailureReports.Remove(entity);
        await dbContext.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
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