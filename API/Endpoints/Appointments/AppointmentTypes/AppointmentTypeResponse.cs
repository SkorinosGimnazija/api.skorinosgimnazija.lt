namespace API.Endpoints.Appointments.AppointmentTypes;

using JetBrains.Annotations;

[PublicAPI]
public record AppointmentTypeResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required DateTime RegistrationEndsAt { get; init; }
}