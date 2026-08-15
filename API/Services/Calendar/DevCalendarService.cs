namespace API.Services.Calendar;

public sealed class DevCalendarService(ILogger<DevCalendarService> logger) : ICalendarService
{
    private readonly List<CalendarEvent> _events =
    [
        new()
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = "(DEV) Sample Event",
            StartDate = DateTime.UtcNow.AddDays(1).ToString("O"),
            EndDate = DateTime.UtcNow.AddDays(1).AddHours(1).ToString("O"),
            AllDay = false
        },
        new()
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = "(DEV) Team Standup",
            StartDate = DateTime.UtcNow.AddDays(2).AddHours(9).ToString("O"),
            EndDate = DateTime.UtcNow.AddDays(2).AddHours(9).AddMinutes(30).ToString("O"),
            AllDay = false
        },
        new()
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = "(DEV) Sprint Planning",
            StartDate = DateTime.UtcNow.AddDays(3).AddHours(10).ToString("O"),
            EndDate = DateTime.UtcNow.AddDays(3).AddHours(12).ToString("O"),
            AllDay = false
        },
        new()
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = "(DEV) Product Review",
            StartDate = DateTime.UtcNow.AddDays(4).AddHours(14).ToString("O"),
            EndDate = DateTime.UtcNow.AddDays(4).AddHours(15).ToString("O"),
            AllDay = false
        },
        new()
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = "(DEV) Design Workshop",
            StartDate = DateTime.UtcNow.AddDays(5).AddHours(11).ToString("O"),
            EndDate = DateTime.UtcNow.AddDays(5).AddHours(13).ToString("O"),
            AllDay = false
        },
        new()
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = "(DEV) Release Day",
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(6)).ToString("O"),
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(6)).ToString("O"),
            AllDay = true
        },
        new()
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = "(DEV) Client Demo",
            StartDate = DateTime.UtcNow.AddDays(7).AddHours(16).ToString("O"),
            EndDate = DateTime.UtcNow.AddDays(7).AddHours(17).ToString("O"),
            AllDay = false
        },
        new()
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = "(DEV) Retrospective",
            StartDate = DateTime.UtcNow.AddDays(8).AddHours(15).ToString("O"),
            EndDate = DateTime.UtcNow.AddDays(8).AddHours(16).ToString("O"),
            AllDay = false
        },
        new()
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = "(DEV) Architecture Sync",
            StartDate = DateTime.UtcNow.AddDays(9).AddHours(13).ToString("O"),
            EndDate = DateTime.UtcNow.AddDays(9).AddHours(14).AddMinutes(30).ToString("O"),
            AllDay = false
        },
        new()
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = "(DEV) Company Holiday",
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10)).ToString("O"),
            EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(15)).ToString("O"),
            AllDay = true
        }
    ];

    public Task<CalendarResponse> CreateAppointmentAsync(
        CalendarAppointmentRequest appointmentRequest, CancellationToken ct)
    {
        logger.LogInformation("Appointment {appointmentRequest} created", appointmentRequest);
        return Task.FromResult(new CalendarResponse
        {
            EventId = Guid.NewGuid().ToString("N"),
            EventLink = appointmentRequest.IsOnline ? "meeting-link" : null
        });
    }

    public Task<CalendarResponse> CreateEventAsync(
        CalendarEventRequest eventRequest, CancellationToken ct)
    {
        var calendarEvent = new CalendarEvent
        {
            Id = Guid.NewGuid().ToString("N"),
            Title = "(DEV) " + eventRequest.Title,
            StartDate = eventRequest.StartDate.ToString("O"),
            EndDate = eventRequest.EndDate.ToString("O"),
            AllDay = eventRequest.AllDay
        };

        _events.Add(calendarEvent);
        logger.LogInformation("Event {calendarEvent} created", calendarEvent);

        return Task.FromResult(new CalendarResponse { EventId = calendarEvent.Id });
    }

    public Task<IEnumerable<CalendarEvent>> ListEventsAsync(
        DateTime start, DateTime end, CancellationToken ct)
    {
        return Task.FromResult(_events.Where(x =>
            DateTime.Parse(x.StartDate) >= start && DateTime.Parse(x.StartDate) <= end));
    }

    public Task<bool> CancelAppointmentAsync(Guid id)
    {
        logger.LogInformation("Appointment {id} deleted", id);
        return Task.FromResult(true);
    }

    public Task<bool> DeleteEventAsync(string id)
    {
        _events.RemoveAll(x => x.Id == id);
        logger.LogInformation("Event {id} deleted", id);
        return Task.FromResult(true);
    }
}