namespace API.Endpoints.Achievements.ListTypes;

public sealed class ListAchievementTypesEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<IEnumerable<AchievementTypeResponse>, AchievementTypeMapper>
{
    public override void Configure()
    {
        Get("/achievements/types");
        Roles(Auth.Role.Teacher);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await dbContext.AchievementTypes
                           .AsNoTracking()
                           .OrderBy(x => x.Id)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}