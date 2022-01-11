namespace SkorinosGimnazija.Application.Appointments.Dtos;

using SkorinosGimnazija.Domain.Entities.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public record AppointmentCreateDto
{
    public int DateId { get; set; }
    public string UserName { get; set; } = default!;
}
