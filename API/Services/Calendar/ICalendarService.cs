namespace API.Services.Calendar;

public interface ICalendarService
{
    Task<CalendarResponse> CreateAppointmentAsync(
        CalendarAppointmentRequest appointmentRequest, CancellationToken ct);

    Task<CalendarResponse> CreateEventAsync(
        CalendarEventRequest eventRequest, CancellationToken ct);

    Task<IEnumerable<CalendarEvent>> ListEventsAsync(
        DateTime start, DateTime end, CancellationToken ct);

    Task<bool> CancelAppointmentAsync(Guid id);

    Task<bool> DeleteEventAsync(string id);
}