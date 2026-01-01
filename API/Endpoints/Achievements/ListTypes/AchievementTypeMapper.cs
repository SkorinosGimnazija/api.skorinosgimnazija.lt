namespace API.Endpoints.Achievements.ListTypes;

using API.Database.Entities.Achievements;

public sealed class AchievementTypeMapper
    : ResponseMapper<AchievementTypeResponse, AchievementType>
{
    public override AchievementTypeResponse FromEntity(AchievementType e)
    {
        return new()
        {
            Id = e.Id,
            Name = e.Name
        };
    }
}