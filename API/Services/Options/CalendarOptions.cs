namespace API.Services.Options;

using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;

public record CalendarOptions
{
    [Required]
    public required string AppointmentsCalendarId { get; [UsedImplicitly] init; }

    [Required]
    public required string EventsCalendarId { get; [UsedImplicitly] init; }

    [Required]
    public required string User { get; [UsedImplicitly] init; }
}