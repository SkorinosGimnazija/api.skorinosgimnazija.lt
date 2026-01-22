namespace API.Endpoints.Appointments.AppointmentTypes.Create;

using System.ComponentModel.DataAnnotations;
using API.Database.Entities.Appointments;
using JetBrains.Annotations;

[PublicAPI]
public record CreateAppointmentTypeRequest
{
    [StringLength(AppointmentTypeConfiguration.NameLength)]
    public required string Name { get; init; }

    [StringLength(AppointmentTypeConfiguration.NameLength)]
    public required string Description { get; init; }

    public required int DurationInMinutes { get; init; }

    public required bool IsOnline { get; init; }

    public required bool IsPublic { get; init; }

    public required DateTime RegistrationEndsAt { get; init; }

    public required List<int> AdditionalInviteeIds { get; init; }

    public required List<int> ExclusiveHostIds { get; init; }
}