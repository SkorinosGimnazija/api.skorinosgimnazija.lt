namespace API.Endpoints.BullyReports.Patch;

public sealed class PatchBullyReportEndpoint(AppDbContext dbContext)
    : Endpoint<PatchBullyReportRequest, EmptyResponse, PatchBullyReportMapper>
{
    public override void Configure()
    {
        Patch("bully-reports");
        Roles(Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(PatchBullyReportRequest req, CancellationToken ct)
    {
        var entity = await dbContext.BullyReports.FindAsync([req.Id], ct);
        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        Map.UpdateEntity(req, entity);
        await dbContext.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}