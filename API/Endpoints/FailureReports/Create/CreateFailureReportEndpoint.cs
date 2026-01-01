namespace API.Endpoints.FailureReports.Create;

using API.Endpoints.FailureReports.Get;

public class CreateFailureReportEndpoint(AppDbContext dbContext)
    : Endpoint<CreateFailureReportRequest, FailureReportResponse, FailureReportMapper>
{
    public override void Configure()
    {
        Post("failure-reports");
    }

    public override async Task HandleAsync(CreateFailureReportRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);

        dbContext.FailureReports.Add(entity);
        await dbContext.SaveChangesAsync(ct);
        await dbContext.Entry(entity).Reference(x => x.Creator).LoadAsync(ct);

        await new CreateFailureReportCommand { ReportId = entity.Id }
            .QueueJobAsync(DateTime.UtcNow.AddMinutes(5), ct: ct);

        await Send.CreatedAtAsync<GetFailureReportEndpoint>(
            new { id = entity.Id },
            Map.FromEntity(entity),
            cancellation: ct);
    }
}