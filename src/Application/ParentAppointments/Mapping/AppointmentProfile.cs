namespace SkorinosGimnazija.Application.Appointments.Mapping;
using AutoMapper;

using SkorinosGimnazija.Domain.Entities.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Appointments;
using Dtos;
using SkorinosGimnazija.Application.ParentAppointments.Dtos;

internal class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<AppointmentDate, AppointmentDateDto>();

        CreateMap<Appointment, AppointmentDto>();
        
        CreateMap<AppointmentCreateDto, Appointment>();

        CreateMap<AppointmentType, AppointmentTypeDto>();
    }
}
