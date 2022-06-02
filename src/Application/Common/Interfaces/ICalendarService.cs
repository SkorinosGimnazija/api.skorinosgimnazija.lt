namespace SkorinosGimnazija.Application.Common.Interfaces;

using Events.Dtos;
using Microsoft.Extensions.Logging;
using SkorinosGimnazija.Application.Common.Models;

public interface ICalendarService
{
    Task<List<EventDto>> GetEventsAsync(DateTime start, DateTime end, CancellationToken ct);

    Task<string> AddEventAsync(string title, DateTime startDate, DateTime endDate, bool allDay);

    Task<AppointmentEventResponse> AddAppointmentAsync(
        string title,
        string description,
        DateTime startDate,
        DateTime endDate,
        params string[] attendeeEmails);

    Task DeleteAppointmentAsync(string eventId);

    Task DeleteEventAsync(string eventId);
}