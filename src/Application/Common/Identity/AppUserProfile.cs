namespace SkorinosGimnazija.Application.Common.Identity;
using AutoMapper;
using SkorinosGimnazija.Application.Courses.Dtos;

using SkorinosGimnazija.Domain.Entities.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Identity;
using Dtos;

public class AppUserProfile : Profile
{
    public AppUserProfile()
    {   
        CreateMap<AppUser, AppUserDto>();
    }
}
