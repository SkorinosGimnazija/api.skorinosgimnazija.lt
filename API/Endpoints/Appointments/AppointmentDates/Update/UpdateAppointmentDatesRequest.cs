namespace API.Endpoints.Appointments.AppointmentDates.Update;

using JetBrains.Annotations;

[PublicAPI]
public record UpdateAppointmentDatesRequest
{
    public required int Id { get; init; }

    public required HashSet<DateTime> Dates { get; init; }
}