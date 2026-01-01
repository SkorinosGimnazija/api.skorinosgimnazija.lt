namespace API.Endpoints.Achievements;

using API.Database.Entities.Achievements;
using API.Endpoints.Achievements.Create;
using API.Endpoints.Users;

public sealed class AchievementMapper
    : Mapper<CreateAchievementRequest, AchievementResponse, Achievement>
{
    public override Achievement ToEntity(CreateAchievementRequest r)
    {
        return new()
        {
            Name = r.Name.Trim(),
            Date = r.Date,
            CreatorId = r.CreatorId,
            ScaleId = r.ScaleId
        };
    }

    public override AchievementResponse FromEntity(Achievement e)
    {
        return new()
        {
            Id = e.Id,
            Name = e.Name,
            Date = e.Date,
            CreatorId = e.Creator.Id,
            CreatorName = e.Creator.Name,
            ScaleId = e.Scale.Id,
            ScaleName = e.Scale.Name,
            Students = e.Students
                .Select(x => new AchievementStudentResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    ClassroomId = x.Classroom.Id,
                    ClassroomName = x.Classroom.Name,
                    AchievementTypeId = x.AchievementType.Id,
                    AchievementTypeName = x.AchievementType.Name
                })
                .OrderBy(x => x.Name)
                .ToList(),
            AdditionalTeachers = e.AdditionalTeachers
                .Select(x => new UserResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    NormalizedName = x.NormalizedName
                })
                .OrderBy(x => x.Name)
                .ToList()
        };
    }

    public override Achievement UpdateEntity(CreateAchievementRequest r, Achievement e)
    {
        e.Name = r.Name.Trim();
        e.Date = r.Date;
        e.ScaleId = r.ScaleId;

        return e;
    }
}