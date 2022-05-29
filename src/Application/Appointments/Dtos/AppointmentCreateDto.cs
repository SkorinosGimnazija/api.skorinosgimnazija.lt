namespace SkorinosGimnazija.Application.Appointments.Dtos;

public record AppointmentPublicCreateDto
{
    public string CaptchaToken { get; init; } = default!;

    public int DateId { get; init; }

    public string UserName { get; init; } = default!;

    public string AttendeeName { get; init; } = default!;

    public string AttendeeEmail { get; init; } = default!;
}