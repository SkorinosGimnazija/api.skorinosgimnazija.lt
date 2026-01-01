namespace API.Endpoints.School.ShortDays.Update;

public sealed class UpdateShortDayEndpoint(AppDbContext dbContext)
    : Endpoint<UpdateShortDayRequest, ShortDayResponse, ShortDayMapper>
{
    public override void Configure()
    {
        Put("school/short-days");
        Roles(Auth.Role.Admin, Auth.Role.Manager);
    }

    public override async Task HandleAsync(UpdateShortDayRequest req, CancellationToken ct)
    {
        var entity = await dbContext.ShortDays.FirstOrDefaultAsync(x => x.Id == req.Id, ct);
        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        Map.UpdateEntity(req, entity);
        await dbContext.SaveChangesAsync(ct);

        await Send.OkAsync(Map.FromEntity(entity), ct);
    }
}