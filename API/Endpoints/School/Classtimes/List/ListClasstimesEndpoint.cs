namespace API.Endpoints.School.Classtimes.List;

public sealed class ListClasstimesEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<IEnumerable<ClasstimeResponse>, ClasstimeMapper>
{
    public override void Configure()
    {
        Get("school/classtimes");
        Roles(Auth.Role.Admin);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await dbContext.Classtimes
                           .AsNoTracking()
                           .OrderBy(x => x.Id)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}