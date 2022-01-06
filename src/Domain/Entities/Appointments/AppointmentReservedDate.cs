namespace SkorinosGimnazija.Domain.Entities.Appointments;

using SkorinosGimnazija.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AppointmentReservedDate
{
    public int Id { get; set; }
    public AppointmentDate Date { get; set; } = default!;
    public int DateId { get; set; }
    public string UserName { get; set; } = default!;
}
