namespace API.Endpoints.School.ShortDays.Create;

using API.Endpoints.School.ShortDays.Get;

public sealed class CreateShortDayEndpoint(AppDbContext dbContext)
    : Endpoint<CreateShortDayRequest, ShortDayResponse, ShortDayMapper>
{
    public override void Configure()
    {
        Post("school/short-days");
        Roles(Auth.Role.Admin, Auth.Role.Manager);
    }

    public override async Task HandleAsync(CreateShortDayRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);

        dbContext.ShortDays.Add(entity);
        await dbContext.SaveChangesAsync(ct);

        await Send.CreatedAtAsync<GetShortDayEndpoint>(
            new { id = entity.Id },
            Map.FromEntity(entity),
            cancellation: ct);
    }
}