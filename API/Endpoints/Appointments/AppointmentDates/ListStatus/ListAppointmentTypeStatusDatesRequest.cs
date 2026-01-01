namespace API.Endpoints.Appointments.AppointmentDates.ListStatus;

using JetBrains.Annotations;

[PublicAPI]
public record ListAppointmentTypeStatusDatesRequest
{
    [RouteParam]
    public required int TypeId { get; init; }

    [RouteParam]
    public required int HostId { get; init; }
}