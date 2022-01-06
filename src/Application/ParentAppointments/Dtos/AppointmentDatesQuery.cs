namespace SkorinosGimnazija.Application.ParentAppointments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record AppointmentDatesQuery
{
    public AppointmentDatesQuery(string userName, string appointmentTypeSlug) 
    {
        UserName = userName;
        AppointmentTypeSlug = appointmentTypeSlug;
    }
     
    public string UserName { get; init; } = default!;

    public string AppointmentTypeSlug { get; init; } = default!;
}
