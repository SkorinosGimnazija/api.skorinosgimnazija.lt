namespace SkorinosGimnazija.Application.Appointments.Mapping;

using AutoMapper;
using Domain.Entities.Appointments;
using Dtos;

internal class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<Appointment, AppointmentDto>();

        CreateMap<Appointment, AppointmentDetailsDto>();

        CreateMap<AppointmentPublicCreateDto, Appointment>();

        CreateMap<AppointmentCreateDto, Appointment>();

        CreateMap<AppointmentDate, AppointmentDateDto>();

        CreateMap<AppointmentType, AppointmentTypeDto>();

        CreateMap<AppointmentTypeCreateDto, AppointmentType>();

        CreateMap<AppointmentDateCreateDto, AppointmentDate>();

        CreateMap<AppointmentTypeEditDto, AppointmentType>();
    }
}