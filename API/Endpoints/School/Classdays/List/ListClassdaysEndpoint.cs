namespace API.Endpoints.School.Classdays.List;

public sealed class ListClassdaysEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<IEnumerable<ClassdayResponse>, ClassdayMapper>
{
    public override void Configure()
    {
        Get("school/classdays");
        Roles(Auth.Role.Admin, Auth.Role.Manager);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await dbContext.Classdays
                           .AsNoTracking()
                           .OrderBy(x => x.Id)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}