namespace API.Services.Calendar;

public sealed class DevCalendarService(ILogger<DevCalendarService> logger) : ICalendarService
{
    private readonly List<CalendarEvent> _events = [];

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