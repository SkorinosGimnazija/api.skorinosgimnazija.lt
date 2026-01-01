namespace API.Endpoints.BullyReports.Create;

using API.Endpoints.BullyReports.Get;

public sealed class CreateBullyReportEndpoint(AppDbContext dbContext)
    : Endpoint<CreateBullyReportRequest, BullyReportResponse, BullyReportMapper>
{
    public override void Configure()
    {
        Post("bully-reports");
        Roles(Auth.Role.Teacher, Auth.Role.Manager, Auth.Role.SocialManager, Auth.Role.Admin);
    }

    public override async Task HandleAsync(CreateBullyReportRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);

        dbContext.BullyReports.Add(entity);
        await dbContext.SaveChangesAsync(ct);
        await dbContext.Entry(entity).Reference(x => x.Creator).LoadAsync(ct);

        await new CreateBullyReportCommand { ReportId = entity.Id }
            .QueueJobAsync(DateTime.UtcNow.AddMinutes(5), ct: ct);

        await Send.CreatedAtAsync<GetBullyReportEndpoint>(
            new { id = entity.Id },
            Map.FromEntity(entity),
            cancellation: ct);
    }
}