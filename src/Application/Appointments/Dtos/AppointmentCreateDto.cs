namespace SkorinosGimnazija.Application.Appointments.Dtos;

public record AppointmentPublicCreateDto
{
    public string CaptchaToken { get; init; } = default!;

    public int DateId { get; set; }

    public string UserName { get; set; } = default!;

    public string AttendeeName { get; set; } = default!;

    public string AttendeeEmail { get; set; } = default!;
}