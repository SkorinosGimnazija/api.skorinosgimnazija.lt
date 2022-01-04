namespace SkorinosGimnazija.Application.Appointments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public  record AppointmentDateDto
{
    public int Id { get; init; }

    public DateTime Date { get; init; }
}
