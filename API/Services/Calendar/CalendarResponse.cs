namespace API.Services.Calendar;

using System.Diagnostics.CodeAnalysis;
using Google.Apis.Calendar.v3.Data;

public record CalendarResponse
{
    public CalendarResponse()
    {
    }

    [SetsRequiredMembers]
    public CalendarResponse(Event eventData)
    {
        Id = eventData.Id;
        EventLink = eventData.HangoutLink;
        Title = eventData.Summary;
        StartDate = eventData.Start.DateTimeRaw ?? eventData.Start.Date;
        EndDate = eventData.End.DateTimeRaw ??
                  DateOnly.Parse(eventData.End.Date).AddDays(-1).ToString("yyyy-MM-dd");
        AllDay = eventData.Transparency == "transparent";
    }

    public required string Id { get; init; }

    public string? EventLink { get; init; }

    public required string Title { get; init; }

    public required string StartDate { get; init; }

    public required string EndDate { get; init; }

    public required bool AllDay { get; init; }
}