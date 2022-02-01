namespace SkorinosGimnazija.Application.Employees.Mapping;

using AutoMapper;
using Common.Identity;
using Domain.Entities.Identity;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(x => x.DisplayName, x => x.MapFrom(z => z.FullName))
            .ForMember(x => x.UserName, x => x.MapFrom(z => z.Id));
    }
}