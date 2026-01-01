namespace API.Endpoints.School.Timetables;

using API.Database.Entities.School;
using API.Endpoints.School.Timetables.Create;

public sealed class TimetableMapper
    : RequestMapper<CreateTimetableRequest, Timetable>
{
    public override Timetable ToEntity(CreateTimetableRequest r)
    {
        return new()
        {
            DayId = r.DayId,
            TimeId = r.TimeId,
            RoomId = r.RoomId,
            ClassName = r.ClassName
        };
    }

    public TimetableOverride ToOverrideEntity(CreateTimetableRequest r, DateOnly date)
    {
        return new()
        {
            Date = date,
            TimeId = r.TimeId,
            RoomId = r.RoomId,
            ClassName = r.ClassName
        };
    }
}