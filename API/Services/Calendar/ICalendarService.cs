namespace API.Services.Calendar;

public interface ICalendarService
{
    Task<CalendarResponse> CreateAppointmentAsync(
        CalendarAppointmentRequest appointmentRequest, CancellationToken ct);

    Task<List<CalendarResponse>> CreateEventAsync(
        IEnumerable<CalendarEventRequest> eventRequests, CancellationToken ct);

    Task<IEnumerable<CalendarResponse>> ListEventsAsync(
        DateTime start, DateTime end, CancellationToken ct);

    Task<bool> CancelAppointmentAsync(Guid id);

    Task<bool> DeleteEventAsync(string id);
}