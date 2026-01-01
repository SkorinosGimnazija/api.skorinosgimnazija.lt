namespace API.Endpoints.Appointments.AppointmentDates.ListAvailable;

using JetBrains.Annotations;

[PublicAPI]
public record ListAppointmentTypeAvailableDatesRequest
{
    [RouteParam]
    public required int TypeId { get; init; }

    [RouteParam]
    public required int HostId { get; init; }
}