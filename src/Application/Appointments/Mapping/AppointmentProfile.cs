namespace SkorinosGimnazija.Application.Appointments.Mapping;

using AutoMapper;
using Common.Models;
using Domain.Entities.Appointments;
using Domain.Entities.Identity;
using Dtos;

internal class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<Appointment, AppointmentDto>();

        CreateMap<AppointmentEventResponse, Appointment>();

        CreateMap<Appointment, AppointmentDetailsDto>();

        CreateMap<AppointmentPublicCreateDto, Appointment>();

        CreateMap<AppointmentCreateDto, Appointment>();

        CreateMap<AppointmentDate, AppointmentDateDto>();

        CreateMap<AppointmentDate, AppointmentDateDetailsDto>();

        CreateMap<AppointmentReservedDateCreateDto, AppointmentReservedDate>();

        CreateMap<AppointmentReservedDate, AppointmentReservedDateDto>();

        CreateMap<AppointmentType, AppointmentTypeDto>();

        CreateMap<AppointmentTypeCreateDto, AppointmentType>();

        CreateMap<AppointmentDateCreateDto, AppointmentDate>();

        CreateMap<AppointmentTypeEditDto, AppointmentType>();

        CreateMap<AppointmentExclusiveHostCreateDto, AppointmentExclusiveHost>();

        CreateMap<AppointmentExclusiveHost, AppointmentExclusiveHostDto>();

        CreateMap<Employee, AppointmentHostDto>()
            .ForMember(x => x.DisplayName, x => x.MapFrom(z => z.FullName))
            .ForMember(x => x.UserName, x => x.MapFrom(z => z.Id));
    }
}