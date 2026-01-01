namespace API.Endpoints.Appointments.Entries.List;

using JetBrains.Annotations;

[PublicAPI]
public record ListAppointmentRequest : PaginationRequest
{
    [QueryParam]
    public int? UserId { get; init; }
}