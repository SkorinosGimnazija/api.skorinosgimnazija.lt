namespace SkorinosGimnazija.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Events.Dtos;
using Pagination;

public interface ICalendarService
{
    Task<List<EventDto>> GetEventsAsync(DateTime start, DateTime end, CancellationToken ct);

    Task<string> AddEventAsync(string title, DateTime startDate, DateTime endDate, bool allDay);

    Task<string> AddAppointmentAsync(
        string title,
        string description,
        DateTime startDate,
        DateTime endDate,
        params string[] attendeeEmails);

    Task DeleteAppointmentAsync(string eventId);

    Task DeleteEventAsync(string eventId);
}
