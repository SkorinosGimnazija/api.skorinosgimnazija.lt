namespace API.Endpoints.Appointments.AppointmentDates;

using JetBrains.Annotations;

[PublicAPI]
public record AppointmentDateResponse
{
    public required int Id { get; init; }

    public required DateTime Date { get; init; }
}