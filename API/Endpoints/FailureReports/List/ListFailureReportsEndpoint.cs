namespace API.Endpoints.FailureReports.List;

using API.Extensions;

public sealed class ListFailureReportsEndpoint(AppDbContext dbContext)
    : Endpoint<ListFailureReportsRequest,
        PaginatedListResponse<FailureReportResponse>,
        FailureReportMapper>
{
    public override void Configure()
    {
        Get("failure-reports");
    }

    public override async Task HandleAsync(ListFailureReportsRequest req, CancellationToken ct)
    {
        var start = req.StartDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var end = req.EndDate.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);

        var entities = await dbContext.FailureReports.AsNoTracking()
                           .Include(x => x.Creator)
                           .Where(x => x.ReportDate >= start && x.ReportDate <= end)
                           .OrderByDescending(x => x.IsFixed == null)
                           .ThenByDescending(x => x.Id)
                           .Select(x => Map.FromEntity(x))
                           .ToPaginatedListAsync(req, ct);

        await Send.OkAsync(entities, ct);
    }
}