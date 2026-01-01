namespace API.Endpoints.Appointments.AppointmentReservedDates.Update;

using JetBrains.Annotations;

[PublicAPI]
public record UpdateAppointmentReservedDatesRequest
{
    public required int DateId { get; init; }

    public required int HostId { get; init; }
}