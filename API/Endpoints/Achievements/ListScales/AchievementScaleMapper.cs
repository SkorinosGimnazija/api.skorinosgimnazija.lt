namespace API.Endpoints.Achievements.ListScales;

using API.Database.Entities.Achievements;

public sealed class AchievementScaleMapper
    : ResponseMapper<AchievementScaleResponse, AchievementScale>
{
    public override AchievementScaleResponse FromEntity(AchievementScale e)
    {
        return new()
        {
            Id = e.Id,
            Name = e.Name
        };
    }
}