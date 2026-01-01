namespace API.Endpoints.Appointments.AppointmentTypes.List;

using JetBrains.Annotations;

[PublicAPI]
public record ListAppointmentTypesRequest
{
    [QueryParam]
    public bool? ShowPrivateOnly { get; init; } = false;
}