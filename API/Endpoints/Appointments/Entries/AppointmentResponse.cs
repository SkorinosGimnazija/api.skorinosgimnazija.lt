namespace API.Endpoints.Appointments.Entries;

using JetBrains.Annotations;

[PublicAPI]
public record AppointmentResponse
{
    public required Guid Id { get; init; }

    public required string? Link { get; init; }

    public required DateTime Date { get; init; }

    public int DateId { get; init; }

    public required string TypeName { get; init; }

    public required string? Note { get; init; }

    public required string HostName { get; init; }

    public required int HostId { get; init; }

    public required string AttendeeName { get; init; }

    public required string AttendeeEmail { get; init; }
}