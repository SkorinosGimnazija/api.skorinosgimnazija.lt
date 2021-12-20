namespace SkorinosGimnazija.Application.Courses.Mapping;
using AutoMapper;
using SkorinosGimnazija.Application.Courses.Dtos;

using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Teacher;

public class CourseProfiles : Profile
{
    public CourseProfiles()
    {
        CreateMap<Course, CourseDto>();

        CreateMap<CourseEditDto, Course>()
            .ForMember(x => x.StartDate, x => x.MapFrom(c => DateOnly.FromDateTime(c.StartDate)))
            .ForMember(x => x.EndDate, x => x.MapFrom(c => DateOnly.FromDateTime(c.EndDate)));

        CreateMap<CourseCreateDto, Course>()
            .ForMember(x=> x.StartDate, x=> x.MapFrom(c=> DateOnly.FromDateTime(c.StartDate)))
            .ForMember(x=> x.EndDate, x=> x.MapFrom(c=> DateOnly.FromDateTime(c.EndDate)));
    }
}
