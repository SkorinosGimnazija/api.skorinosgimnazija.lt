namespace API.Endpoints.School.Classtimes;

using API.Database.Entities.School;
using API.Endpoints.School.Classtimes.Upsert;

public sealed class ClasstimeMapper : Mapper<UpsertClasstimeRequest, ClasstimeResponse, Classtime>
{
    public override Classtime ToEntity(UpsertClasstimeRequest r)
    {
        return new()
        {
            Id = r.Id,
            StartTime = r.StartTime,
            StartTimeShort = r.StartTimeShort,
            EndTime = r.EndTime,
            EndTimeShort = r.EndTimeShort
        };
    }

    public override ClasstimeResponse FromEntity(Classtime e)
    {
        return new()
        {
            Id = e.Id,
            StartTime = e.StartTime.ToShortTimeString(),
            StartTimeShort = e.StartTimeShort?.ToShortTimeString(),
            EndTime = e.EndTime.ToShortTimeString(),
            EndTimeShort = e.EndTimeShort?.ToShortTimeString()
        };
    }

    public override Classtime UpdateEntity(UpsertClasstimeRequest r, Classtime e)
    {
        e.StartTime = r.StartTime;
        e.StartTimeShort = r.StartTimeShort;
        e.EndTime = r.EndTime;
        e.EndTimeShort = r.EndTimeShort;

        return e;
    }
}