namespace API.Endpoints.Achievements.Create;

using API.Database.Entities.Achievements;
using API.Database.Entities.Authorization;
using API.Endpoints.Achievements.Get;

public sealed class CreateAchievementEndpoint(AppDbContext dbContext)
    : Endpoint<CreateAchievementRequest, AchievementResponse, AchievementMapper>
{
    public override void Configure()
    {
        Post("achievements");
        Roles(Auth.Role.Teacher);
    }

    public override async Task HandleAsync(CreateAchievementRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);

        entity.AdditionalTeachers = req.AdditionalTeachers
            .Select(id => dbContext.Attach(new AppUser { Id = id }).Entity)
            .ToList();

        entity.Students = req.Students
            .Select(s =>
                new AchievementStudent
                {
                    Name = s.Name.Trim(),
                    ClassroomId = s.ClassroomId,
                    AchievementTypeId = s.AchievementTypeId
                })
            .ToList();

        dbContext.Achievements.Add(entity);
        await dbContext.SaveChangesAsync(ct);

        entity = await dbContext.Achievements.AsNoTracking()
                     .Include(x => x.Creator)
                     .Include(x => x.Scale)
                     .Include(x => x.Students)
                     .ThenInclude(s => s.Classroom)
                     .Include(x => x.Students)
                     .ThenInclude(s => s.AchievementType)
                     .FirstAsync(x => x.Id == entity.Id, ct);

        await Send.CreatedAtAsync<GetAchievementEndpoint>(
            new { id = entity.Id },
            Map.FromEntity(entity),
            cancellation: ct);
    }
}