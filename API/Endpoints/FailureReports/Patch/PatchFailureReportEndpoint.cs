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
        var fixStatusModified = dbContext.Entry(entity).Property(x => x.IsFixed).IsModified;

        await dbContext.SaveChangesAsync(ct);

        if (!string.IsNullOrWhiteSpace(req.Note) || fixStatusModified)
        {
            await new PatchFailureReportCommand
                {
                    ReportId = entity.Id,
                    FixerId = req.FixerId,
                    Note = req.Note
                }
                .QueueJobAsync(ct: ct);
        }

        await Send.NoContentAsync(ct);
    }
}