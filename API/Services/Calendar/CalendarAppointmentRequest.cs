namespace API.Services.Calendar;

public record CalendarAppointmentRequest
{
    public required Guid Id { get; init; }

    public required string Title { get; init; }

    public required bool IsOnline { get; init; }

    public required DateTime StartDate { get; init; }

    public required int DurationInMinutes { get; init; }

    public required List<string> AdditionalInvitees { get; init; }

    public required string HostEmail { get; init; }

    public required string HostName { get; init; }

    public required string? Note { get; init; }

    public required string AttendeeEmail { get; init; }

    public required string AttendeeName { get; init; }
}