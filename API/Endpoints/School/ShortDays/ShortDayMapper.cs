namespace API.Endpoints.School.ShortDays;

using API.Database.Entities.School;
using API.Endpoints.School.ShortDays.Create;

public sealed class ShortDayMapper
    : Mapper<CreateShortDayRequest, ShortDayResponse, ShortDay>
{
    public override ShortDay ToEntity(CreateShortDayRequest r)
    {
        return new()
        {
            Date = r.Date
        };
    }

    public override ShortDayResponse FromEntity(ShortDay e)
    {
        return new()
        {
            Id = e.Id,
            Date = e.Date
        };
    }

    public override ShortDay UpdateEntity(CreateShortDayRequest r, ShortDay e)
    {
        e.Date = r.Date;

        return e;
    }
}