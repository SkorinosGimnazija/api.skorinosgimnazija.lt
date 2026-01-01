namespace API.Endpoints.School.Classrooms;

using API.Database.Entities.School;
using API.Endpoints.School.Classrooms.Upsert;

public sealed class ClassroomMapper : Mapper<UpsertClassroomRequest, ClassroomResponse, Classroom>
{
    public override Classroom ToEntity(UpsertClassroomRequest r)
    {
        return new()
        {
            Id = r.Id,
            Name = r.Name
        };
    }

    public override ClassroomResponse FromEntity(Classroom e)
    {
        return new()
        {
            Id = e.Id,
            Name = e.Name
        };
    }

    public override Classroom UpdateEntity(UpsertClassroomRequest r, Classroom e)
    {
        e.Name = r.Name;

        return e;
    }
}