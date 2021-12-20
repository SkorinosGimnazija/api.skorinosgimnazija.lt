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

        CreateMap<CourseEditDto, Course>();

        CreateMap<CourseCreateDto, Course>();
    }
}
