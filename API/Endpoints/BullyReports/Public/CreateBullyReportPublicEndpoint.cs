namespace API.Endpoints.BullyReports.Public;

using API.Endpoints.BullyReports.Create;

public sealed class CreateBullyReportPublicEndpoint(AppDbContext dbContext)
    : Endpoint<CreateBullyReportPublicRequest, EmptyResponse, BullyReportMapper>
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

        // dont sent created entity ?
        await Send.ResponseAsync(new(), StatusCodes.Status201Created, ct);
    }
}