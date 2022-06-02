namespace SkorinosGimnazija.Application.Appointments.Mapping;

using AutoMapper;
using Domain.Entities.Appointments;
using Dtos;
using SkorinosGimnazija.Application.Employees.Dtos;
using SkorinosGimnazija.Domain.Entities.Identity;

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

        CreateMap<AppointmentExclusiveHostCreateDto, AppointmentExclusiveHost>();

        CreateMap<AppointmentExclusiveHost, AppointmentExclusiveHostDto>();

        CreateMap<Employee, AppointmentHostDto>()
            .ForMember(x => x.DisplayName, x => x.MapFrom(z => z.FullName))
            .ForMember(x => x.UserName, x => x.MapFrom(z => z.Id));
    }
}