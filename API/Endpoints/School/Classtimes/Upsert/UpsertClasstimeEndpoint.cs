namespace API.Endpoints.School.Classtimes.Upsert;

using API.Endpoints.School.Classtimes.Get;

public sealed class UpsertClasstimeEndpoint(AppDbContext dbContext)
    : Endpoint<UpsertClasstimeRequest, ClasstimeResponse, ClasstimeMapper>
{
    public override void Configure()
    {
        Put("school/classtimes");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(UpsertClasstimeRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Classtimes.FirstOrDefaultAsync(x => x.Id == req.Id, ct);
        if (entity is not null)
        {
            Map.UpdateEntity(req, entity);
            await dbContext.SaveChangesAsync(ct);

            await Send.OkAsync(Map.FromEntity(entity), ct);
            return;
        }

        var createEntity = Map.ToEntity(req);
        dbContext.Classtimes.Add(createEntity);
        await dbContext.SaveChangesAsync(ct);

        await Send.CreatedAtAsync<GetClasstimeEndpoint>(
            new { id = createEntity.Id },
            Map.FromEntity(createEntity),
            cancellation: ct);
    }
}