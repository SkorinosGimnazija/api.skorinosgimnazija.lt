namespace SkorinosGimnazija.Application.Employees.Mapping;
using AutoMapper;
using SkorinosGimnazija.Application.Common.Identity;
using SkorinosGimnazija.Application.Courses.Dtos;

using SkorinosGimnazija.Domain.Entities.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
