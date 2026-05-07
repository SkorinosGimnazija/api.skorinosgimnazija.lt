namespace API.Database.Entities.Appointments;

public class AppointmentReservedDate
{
    public int DateId { get; set; }

    public AppointmentDate Date { get; set; } = null!;

    public int HostId { get; set; }
}