namespace API.Endpoints.Appointments.AppointmentReservedDates.Update;

using JetBrains.Annotations;

[PublicAPI]
public record UpdateAppointmentReservedDatesRequest
{
    public required int Id { get; init; }

    public required int HostId { get; init; }

    public required HashSet<int> DateIds { get; init; }
}