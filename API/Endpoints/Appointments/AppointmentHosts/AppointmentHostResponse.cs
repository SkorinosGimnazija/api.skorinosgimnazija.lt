namespace API.Endpoints.Appointments.AppointmentHosts;

using JetBrains.Annotations;

[PublicAPI]
public record AppointmentHostResponse
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required string NormalizedName { get; init; }
}