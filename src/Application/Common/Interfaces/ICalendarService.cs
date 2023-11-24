namespace SkorinosGimnazija.Application.Common.Interfaces;

using Events.Dtos;
using Models;

public interface ICalendarService
{
    Task<List<EventDto>> GetEventsAsync(DateTime start, DateTime end, CancellationToken ct);

    Task<string> AddEventAsync(string title, DateTime startDate, DateTime endDate, bool allDay);

    Task<AppointmentEventResponse> AddAppointmentAsync(AppointmentEvent appointment);

    Task DeleteAppointmentAsync(string eventId);

    Task<bool> DeleteEventAsync(string eventId);
}