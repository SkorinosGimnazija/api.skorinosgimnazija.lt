namespace SkorinosGimnazija.Infrastructure.Calendar;

using System.Net;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Events.Dtos;
using Domain.Options;
using Google;
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
        request.TimeZone = "UTC";

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
                DateTimeDateTimeOffset = !allDay ? startDate : null
            },
            End = new()
            {
                Date = allDay ? endDate.AddDays(1).ToString("yyyy-MM-dd") : null,
                DateTimeDateTimeOffset = !allDay ? endDate : null
            },
            Transparency = allDay ? "transparent" : "opaque"
        };

        var request = _calendarService.Events.Insert(@event, _eventsCalendarId);
        var response = await request.ExecuteAsync();

        return response.Id;
    }

    public async Task<AppointmentEventResponse> AddAppointmentAsync(AppointmentEvent appointment)
    {
        if (_env.IsDevelopment())
        {
            return new() { EventId = Guid.NewGuid().ToString(), EventMeetingLink = Guid.NewGuid().ToString() };
        }

        var @event = new Event
        {
            Summary = appointment.Title,
            Description = appointment.Description,
            Start = new() { DateTimeDateTimeOffset = appointment.StartDate },
            End = new() { DateTimeDateTimeOffset = appointment.EndDate },
            Location = "Vilniaus Pranciškaus Skorinos gimnazija",
            Attendees = appointment.AttendeeEmails.Distinct()
                .Select(email => new EventAttendee { Email = email })
                .ToArray(),
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

        if (appointment.IsOnline)
        {
            request.ConferenceDataVersion = 1;
            @event.Location = null;
        }

        var response = await request.ExecuteAsync();

        return new() { EventId = response.Id, EventMeetingLink = response.HangoutLink };
    }

    public async Task<bool> DeleteEventAsync(string eventId)
    {
        try
        {
            var request = _calendarService.Events.Delete(_eventsCalendarId, eventId);
            await request.ExecuteAsync();
        }
        catch (GoogleApiException e) when (e.HttpStatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }

        return true;
    }

    public async Task DeleteAppointmentAsync(string eventId)
    {
        var request = _calendarService.Events.Delete(_appointmentsCalendarId, eventId);

        request.SendUpdates = EventsResource.DeleteRequest.SendUpdatesEnum.All;

        await request.ExecuteAsync();
    }
}