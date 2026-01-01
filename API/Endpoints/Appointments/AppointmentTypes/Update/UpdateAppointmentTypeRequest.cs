namespace API.Endpoints.Appointments.AppointmentTypes.Update;

using API.Endpoints.Appointments.AppointmentTypes.Create;
using JetBrains.Annotations;

[PublicAPI]
public record UpdateAppointmentTypeRequest : CreateAppointmentTypeRequest
{
    public required int Id { get; init; }
}