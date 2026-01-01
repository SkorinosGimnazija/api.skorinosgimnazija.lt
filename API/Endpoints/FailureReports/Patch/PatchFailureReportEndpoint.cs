namespace API.Endpoints.FailureReports.Patch;

public sealed class PatchFailureReportEndpoint(AppDbContext dbContext)
    : Endpoint<PatchFailureReportRequest, EmptyResponse, PatchFailureReportMapper>
{
    public override void Configure()
    {
        Patch("failure-reports");
        Roles(Auth.Role.TechManager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(PatchFailureReportRequest req, CancellationToken ct)
    {
        var entity = await dbContext.FailureReports.FindAsync([req.Id], ct);
        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        Map.UpdateEntity(req, entity);
        await dbContext.SaveChangesAsync(ct);

        await new PatchFailureReportCommand
            {
                ReportId = entity.Id,
                FixerId = req.FixerId,
                Note = req.Note?.Trim()
            }
            .QueueJobAsync(ct: ct);

        await Send.NoContentAsync(ct);
    }
}