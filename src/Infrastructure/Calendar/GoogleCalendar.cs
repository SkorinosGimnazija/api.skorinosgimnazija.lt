namespace SkorinosGimnazija.Infrastructure.Calendar;

using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Events.Dtos;
using Domain.Options;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

public class GoogleCalendar : ICalendarService
{
    private readonly string _appointmentsCalendarId;
    private readonly CalendarService _calendarService;
    private readonly IWebHostEnvironment _env;
    private readonly string _eventsCalendarId;

    public GoogleCalendar(
        IOptions<GoogleOptions> googleOptions,
        IOptions<CalendarOptions> calendarOptions,
        IWebHostEnvironment env)
    {
        _env = env;
        _appointmentsCalendarId = calendarOptions.Value.AppointmentsCalendarId;
        _eventsCalendarId = calendarOptions.Value.EventsCalendarId;

        _calendarService = new(new()
        {
            HttpClientInitializer = GoogleCredential.FromJson(googleOptions.Value.Credential)
                .CreateScoped(CalendarService.ScopeConstants.CalendarEvents)
                .CreateWithUser(calendarOptions.Value.User)
        });
    }

    public async Task<List<EventDto>> GetEventsAsync(DateTime start, DateTime end, CancellationToken ct)
    {
        var request = _calendarService.Events.List(_eventsCalendarId);

        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        request.SingleEvents = true;
        request.TimeMin = start;
        request.TimeMax = end;

        var response = await request.ExecuteAsync(ct);

        return response.Items.Select(x => new EventDto
            {
                Id = x.Id,
                Title = x.Summary,
                StartDateTime = x.Start.DateTimeRaw,
                StartDate = x.Start.Date,
                EndDateTime = x.End.DateTimeRaw,
                EndDate = x.End.Date is not null ? DateOnly.Parse(x.End.Date).AddDays(-1).ToString("yyyy-MM-dd") : null
            })
            .ToList();
    }

    public async Task<string> AddEventAsync(string title, DateTime startDate, DateTime endDate, bool allDay)
    {
        var @event = new Event
        {
            Summary = title,
            Start = new()
            {
                Date = allDay ? startDate.ToString("yyyy-MM-dd") : null,
                DateTime = !allDay ? startDate : null
            },
            End = new()
            {
                Date = allDay ? endDate.ToString("yyyy-MM-dd") : null,
                DateTime = !allDay ? endDate : null
            }
        };

        var request = _calendarService.Events.Insert(@event, _eventsCalendarId);
        var response = await request.ExecuteAsync();

        return response.Id;
    }

    public async Task<AppointmentEventResponse> AddAppointmentAsync(
        string title,
        string description,
        DateTime startDate,
        DateTime endDate,
        params string[] attendeeEmails)
    {
        if (_env.IsDevelopment())
        {
            return new() { EventId = Guid.NewGuid().ToString(), EventMeetingLink = Guid.NewGuid().ToString() };
        }

        var @event = new Event
        {
            Summary = title,
            Description = description,
            Start = new() { DateTime = startDate },
            End = new() { DateTime = endDate },
            Attendees = attendeeEmails.Distinct().Select(email => new EventAttendee { Email = email }).ToArray(),
            GuestsCanInviteOthers = false,
            ConferenceData = new()
            {
                CreateRequest = new()
                {
                    RequestId = Guid.NewGuid().ToString(),
                    ConferenceSolutionKey = new()
                    {
                        Type = "hangoutsMeet"
                    }
                }
            }
        };

        var request = _calendarService.Events.Insert(@event, _appointmentsCalendarId);

        request.SendUpdates = EventsResource.InsertRequest.SendUpdatesEnum.All;
        request.ConferenceDataVersion = 1;

        var response = await request.ExecuteAsync();

        return new() { EventId = response.Id, EventMeetingLink = response.HangoutLink };
    }

    public async Task DeleteEventAsync(string eventId)
    {
        var request = _calendarService.Events.Delete(_eventsCalendarId, eventId);
        await request.ExecuteAsync();
    }

    public async Task DeleteAppointmentAsync(string eventId)
    {
        var request = _calendarService.Events.Delete(_appointmentsCalendarId, eventId);

        request.SendUpdates = EventsResource.DeleteRequest.SendUpdatesEnum.All;

        await request.ExecuteAsync();
    }
}