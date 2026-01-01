namespace API.Endpoints.Appointments.AppointmentDates.Update;

using JetBrains.Annotations;

[PublicAPI]
public record UpdateAppointmentDatesRequest
{
    public required int TypeId { get; init; }

    public required DateTime Date { get; init; }
}