namespace SkorinosGimnazija.Domain.Entities.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Identity;

public  class ParentAppointment
{
    public int Id { get; set; }
    public string EventId { get; set; } = default!;
    public int DateId { get; set; }
    public AppointmentDate Date { get; set; } = default!;
    public int UserId { get; set; }  
    public string UserName { get; set; } = default!;
    public AppUser User { get; set; } = default!; 
    public string ParentName { get; set; } = default!;
    public string ParentEmail{ get; set; } = default!;

}
 