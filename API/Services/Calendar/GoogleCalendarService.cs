namespace API.Services.Calendar;

using System.Net;
using API.Services.Options;
using Google;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Microsoft.Extensions.Options;

public sealed class GoogleCalendarService(
    IOptions<GoogleOptions> googleOptions,
    IOptions<CalendarOptions> calendarOptions)
    : ICalendarService
{
    private readonly string _appointmentsCalendarId = calendarOptions.Value.AppointmentsCalendarId;

    private readonly CalendarService _calendarService = new(new()
    {
        HttpClientInitializer = googleOptions.Value.CreateCredential()
            .CreateScoped(CalendarService.ScopeConstants.CalendarEvents)
            .CreateWithUser(calendarOptions.Value.User)
    });

    private readonly string _eventsCalendarId = calendarOptions.Value.EventsCalendarId;

    public async Task<IEnumerable<CalendarResponse>> ListEventsAsync(
        DateTime start, DateTime end, CancellationToken ct)
    {
        var request = _calendarService.Events.List(_eventsCalendarId);

        request.TimeZone = TimeZoneInfo.Utc.Id;
        request.TimeMinDateTimeOffset = new DateTimeOffset(start, TimeSpan.Zero);
        request.TimeMaxDateTimeOffset = new DateTimeOffset(end, TimeSpan.Zero);
        request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;
        request.SingleEvents = true;

        var response = await request.ExecuteAsync(ct);

        return response.Items.Select(x => new CalendarResponse(x));
    }

    public async Task<List<CalendarResponse>> CreateEventAsync(
        IEnumerable<CalendarEventRequest> eventRequests, CancellationToken ct)
    {
        var results = new List<CalendarResponse>();

        // no parallel to avoid rate limit
        foreach (var eventRequest in eventRequests)
        {
            var calendarEvent = new Event
            {
                Summary = eventRequest.Title,
                Start = new()
                {
                    Date = eventRequest.AllDay ? eventRequest.StartDate.ToString("yyyy-MM-dd") : null,
                    DateTimeDateTimeOffset = !eventRequest.AllDay ? eventRequest.StartDate : null
                },
                End = new()
                {
                    Date = eventRequest.AllDay
                               ? eventRequest.EndDate.AddDays(1).ToString("yyyy-MM-dd")
                               : null,
                    DateTimeDateTimeOffset = !eventRequest.AllDay ? eventRequest.EndDate : null
                },
                Transparency = eventRequest.AllDay ? "transparent" : "opaque"
            };

            var request = _calendarService.Events.Insert(calendarEvent, _eventsCalendarId);
            var response = await request.ExecuteAsync(ct);

            results.Add(new(response));
        }

        return results;
    }

    public async Task<CalendarResponse> CreateAppointmentAsync(
        CalendarAppointmentRequest appointmentRequest, CancellationToken ct)
    {
        var calendarEvent = new Event
        {
            Id = appointmentRequest.Id.ToString("N"),
            Summary = appointmentRequest.Title,
            Description = GetDescription(appointmentRequest),
            Start = new() { DateTimeDateTimeOffset = appointmentRequest.StartDate },
            End = new()
            {
                DateTimeDateTimeOffset =
                    appointmentRequest.StartDate.AddMinutes(appointmentRequest.DurationInMinutes)
            },
            Location = "Vilniaus Pranciškaus Skorinos gimnazija",
            Attendees = new[] { appointmentRequest.HostEmail, appointmentRequest.AttendeeEmail }
                .Union(appointmentRequest.AdditionalInvitees)
                .Select(email => new EventAttendee { Email = email })
                .ToList(),
            GuestsCanInviteOthers = false,
            GuestsCanSeeOtherGuests = true,
            ConferenceData = new()
            {
                CreateRequest = new()
                {
                    RequestId = Guid.CreateVersion7().ToString(),
                    ConferenceSolutionKey = new()
                    {
                        Type = "hangoutsMeet"
                    }
                }
            }
        };

        var request = _calendarService.Events.Insert(calendarEvent, _appointmentsCalendarId);

        request.SendUpdates = EventsResource.InsertRequest.SendUpdatesEnum.All;

        if (appointmentRequest.IsOnline)
        {
            request.ConferenceDataVersion = 1;
            calendarEvent.Location = null;
        }

        var response = await request.ExecuteAsync(ct);

        return new(response);
    }

    public async Task<bool> DeleteEventAsync(string id)
    {
        try
        {
            var request = _calendarService.Events.Delete(_eventsCalendarId, id);
            await request.ExecuteAsync();
        }
        catch (GoogleApiException e) when
            (e.HttpStatusCode is HttpStatusCode.NotFound or HttpStatusCode.Gone)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> CancelAppointmentAsync(Guid id)
    {
        try
        {
            var request = _calendarService.Events.Delete(_appointmentsCalendarId, id.ToString("N"));
            request.SendUpdates = EventsResource.DeleteRequest.SendUpdatesEnum.All;
            await request.ExecuteAsync();
        }
        catch (GoogleApiException e) when
            (e.HttpStatusCode is HttpStatusCode.NotFound or HttpStatusCode.Gone)
        {
            return false;
        }

        return true;
    }

    private string GetDescription(CalendarAppointmentRequest appointment)
    {
        var description =
            $"{appointment.HostName} // {WebUtility.HtmlEncode(appointment.AttendeeName)}";

        if (!string.IsNullOrWhiteSpace(appointment.Note))
        {
            description += $" ({WebUtility.HtmlEncode(appointment.Note)})";
        }

        return description;
    }
}