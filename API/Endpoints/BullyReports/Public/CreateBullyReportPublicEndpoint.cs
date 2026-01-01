namespace API.Endpoints.BullyReports.Public;

using API.Endpoints.BullyReports.Create;

public sealed class CreateBullyReportPublicEndpoint(AppDbContext dbContext)
    : Endpoint<CreateBullyReportPublicRequest, BullyReportResponse, BullyReportMapper>
{
    public override void Configure()
    {
        Post("public/bully-reports");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBullyReportPublicRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);

        dbContext.BullyReports.Add(entity);
        await dbContext.SaveChangesAsync(ct);

        await new CreateBullyReportCommand { ReportId = entity.Id }
            .QueueJobAsync(DateTime.UtcNow.AddMinutes(5), ct: ct);

        await Send.StatusCodeAsync(StatusCodes.Status201Created, ct);
    }
}