namespace SkorinosGimnazija.Domain.Entities.Appointments;

public class Appointment
{
    public int Id { get; set; }

    public string EventId { get; set; } = string.Empty;

    public string? EventMeetingLink { get; set; }

    public int DateId { get; set; }

    public AppointmentDate Date { get; set; } = default!;

    public string UserName { get; set; } = default!;

    public string UserDisplayName { get; set; } = default!;

    public string AttendeeName { get; set; } = default!;

    public string AttendeeEmail { get; set; } = default!;

    public string? AttendeeUserName { get; set; }
}