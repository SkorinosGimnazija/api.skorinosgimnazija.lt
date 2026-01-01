namespace API.Endpoints.School.Classrooms.Upsert;

using API.Endpoints.School.Classrooms.Get;

public sealed class UpsertClassroomEndpoint(AppDbContext dbContext)
    : Endpoint<UpsertClassroomRequest, ClassroomResponse, ClassroomMapper>
{
    public override void Configure()
    {
        Put("school/classrooms");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(UpsertClassroomRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Classrooms.FirstOrDefaultAsync(x => x.Id == req.Id, ct);
        if (entity is not null)
        {
            Map.UpdateEntity(req, entity);
            await dbContext.SaveChangesAsync(ct);

            await Send.OkAsync(Map.FromEntity(entity), ct);
            return;
        }

        var createEntity = Map.ToEntity(req);
        dbContext.Classrooms.Add(createEntity);
        await dbContext.SaveChangesAsync(ct);

        await Send.CreatedAtAsync<GetClassroomEndpoint>(
            new { id = createEntity.Id },
            Map.FromEntity(createEntity),
            cancellation: ct);
    }
}