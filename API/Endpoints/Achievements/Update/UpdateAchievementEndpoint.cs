namespace API.Endpoints.Achievements.Update;

using API.Database.Entities.Achievements;
using API.Extensions;

public sealed class UpdateAchievementEndpoint(AppDbContext dbContext)
    : Endpoint<UpdateAchievementRequest, AchievementResponse, AchievementMapper>
{
    public override void Configure()
    {
        Put("achievements");
        Roles(Auth.Role.Teacher);
    }

    public override async Task HandleAsync(UpdateAchievementRequest req, CancellationToken ct)
    {
        var entity = await dbContext.Achievements
                         .Include(x => x.AdditionalTeachers)
                         .Include(x => x.Students)
                         .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (entity is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        if (!HasAccess(entity))
        {
            await Send.ForbiddenAsync(ct);
            return;
        }

        Map.UpdateEntity(req, entity);

        entity.AdditionalTeachers = await dbContext.Users
                                        .Where(x => req.AdditionalTeachers.Contains(x.Id))
                                        .ToListAsync(ct);

        entity.Students = req.Students
            .Select(x =>
                new AchievementStudent
                {
                    Name = x.Name.Trim(),
                    ClassroomId = x.ClassroomId,
                    AchievementTypeId = x.AchievementTypeId
                })
            .ToList();

        await dbContext.SaveChangesAsync(ct);

        var result = await dbContext.Achievements.AsNoTracking()
                         .Include(x => x.Creator)
                         .Include(x => x.Scale)
                         .Include(x => x.Students)
                         .ThenInclude(s => s.Classroom)
                         .Include(x => x.Students)
                         .ThenInclude(s => s.AchievementType)
                         .FirstAsync(x => x.Id == entity.Id, ct);

        await Send.OkAsync(Map.FromEntity(result), ct);
    }

    private bool HasAccess(Achievement entity)
    {
        var canAccessRequested = User.HasId(entity.CreatorId);
        if (canAccessRequested)
        {
            return true;
        }

        var canAccessAny = User.IsAdmin();
        if (canAccessAny)
        {
            return true;
        }

        return false;
    }
}