namespace API.Endpoints.School.Classdays;

using API.Database.Entities.School;

public sealed class ClassdayMapper : ResponseMapper<ClassdayResponse, Classday>
{
    public override ClassdayResponse FromEntity(Classday e)
    {
        return new()
        {
            Id = e.Id,
            Name = e.Name
        };
    }
}