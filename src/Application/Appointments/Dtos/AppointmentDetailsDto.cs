namespace SkorinosGimnazija.Application.ParentAppointments.Dtos;

using SkorinosGimnazija.Application.Appointments.Dtos;
using SkorinosGimnazija.Domain.Entities.Appointments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public  record AppointmentDetailsDto
{
    public int Id { get; init; }
    public string EventId { get; init; } = default!;
    public int DateId { get; init; }
    public string UserName { get; init; } = default!;
    public string AttendeeName { get; init; } = default!;
    public string AttendeeEmail { get; init; } = default!;
    public AppointmentDateDto Date { get; set; } = default!;
}
