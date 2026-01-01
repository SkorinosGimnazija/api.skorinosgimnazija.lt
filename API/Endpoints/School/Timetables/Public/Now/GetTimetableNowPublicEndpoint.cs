namespace API.Endpoints.School.Timetables.Public.Now;

using API.Extensions;

public sealed class GetTimetableNowPublicEndpoint(AppDbContext dbContext, TimeProvider timeProvider)
    : EndpointWithoutRequest<TimetableNowResponse>
{
    public override void Configure()
    {
        Get("public/school/timetable/now");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var ltNow = timeProvider.LtNow;
        var date = DateOnly.FromDateTime(ltNow);
        var time = TimeOnly.FromDateTime(ltNow);
        var dayId = (int) date.DayOfWeek;

        var isShortDay = await dbContext.ShortDays
                             .AsNoTracking()
                             .AnyAsync(x => x.Date == date, ct);

        var classtime = await dbContext.Classtimes
                            .AsNoTracking()
                            .Where(x => (isShortDay ? x.EndTimeShort : x.EndTime) > time)
                            .OrderBy(x => x.Id)
                            .Select(x => new
                            {
                                x.Id,
                                StartTime = isShortDay ? x.StartTimeShort : x.StartTime,
                                EndTime = isShortDay ? x.EndTimeShort : x.EndTime
                            })
                            .FirstOrDefaultAsync(ct);

        if (classtime is null)
        {
            await Send.NoContentAsync(ct);
            return;
        }

        var isOverrideActive = await dbContext.TimetableOverrides
                                   .AsNoTracking()
                                   .AnyAsync(x => x.Date == date, ct);

        List<TimetableNowResponse.RoomClass> roomClasses;

        if (isOverrideActive)
        {
            roomClasses = await dbContext.TimetableOverrides
                              .AsNoTracking()
                              .Where(x => x.Date == date && x.TimeId == classtime.Id)
                              .OrderBy(x => x.RoomId)
                              .Select(x => new TimetableNowResponse.RoomClass
                              {
                                  RoomId = x.RoomId,
                                  RoomName = x.Room.Name,
                                  ClassName = x.ClassName
                              })
                              .ToListAsync(ct);
        }
        else
        {
            roomClasses = await dbContext.Timetable
                              .AsNoTracking()
                              .Where(x => x.DayId == dayId && x.TimeId == classtime.Id)
                              .OrderBy(x => x.RoomId)
                              .Select(x => new TimetableNowResponse.RoomClass
                              {
                                  RoomId = x.RoomId,
                                  RoomName = x.Room.Name,
                                  ClassName = x.ClassName
                              })
                              .ToListAsync(ct);
        }

        var response = new TimetableNowResponse
        {
            CurrentTime = time.ToShortTimeString(),
            CurrentClass = new()
            {
                Id = classtime.Id,
                StartTime = classtime.StartTime.ToShortTimeString(),
                EndTime = classtime.EndTime.ToShortTimeString()
            },
            RoomClasses = roomClasses
        };

        await Send.OkAsync(response, ct);
    }
}