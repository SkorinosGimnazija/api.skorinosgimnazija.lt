namespace API.Endpoints.Appointments.AppointmentTypes;

using JetBrains.Annotations;

[PublicAPI]
public record AppointmentTypeDetailedResponse : AppointmentTypeResponse
{
    public required int DurationInMinutes { get; init; }

    public required bool IsPublic { get; init; }

    public required bool IsOnline { get; init; }

    public required List<int> AdditionalInviteeIds { get; init; }

    public required List<int> ExclusiveHostIds { get; init; }
}