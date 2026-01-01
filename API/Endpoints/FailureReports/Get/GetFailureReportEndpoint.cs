namespace API.Endpoints.FailureReports.Get;

public sealed class GetFailureReportEndpoint(AppDbContext dbContext)
    : Endpoint<RouteIdRequest, FailureReportResponse, FailureReportMapper>
{
    public override void Configure()
    {
        Get("failure-reports/{id}");
    }

    public override async Task HandleAsync(RouteIdRequest req, CancellationToken ct)
    {
        var entity = await dbContext.FailureReports.AsNoTracking()
                         .Include(x => x.Creator)
                         .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}