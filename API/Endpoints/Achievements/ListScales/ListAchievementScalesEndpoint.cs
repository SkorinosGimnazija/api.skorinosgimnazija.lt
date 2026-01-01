namespace API.Endpoints.Achievements.ListScales;

public sealed class ListAchievementScalesEndpoint(AppDbContext dbContext)
    : EndpointWithoutRequest<IEnumerable<AchievementScaleResponse>, AchievementScaleMapper>
{
    public override void Configure()
    {
        Get("/achievements/scales");
        Roles(Auth.Role.Teacher);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var entities = await dbContext.AchievementScales
                           .AsNoTracking()
                           .OrderBy(x => x.Id)
                           .ToListAsync(ct);

        await Send.OkAsync(entities.Select(Map.FromEntity), ct);
    }
}