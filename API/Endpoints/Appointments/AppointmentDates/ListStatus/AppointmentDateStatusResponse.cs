namespace API.Endpoints.Appointments.AppointmentDates.ListStatus;

using JetBrains.Annotations;

[PublicAPI]
public record AppointmentDateStatusResponse : AppointmentDateResponse
{
    public required bool IsRegistered { get; init; }

    public required bool IsReserved { get; init; }
}