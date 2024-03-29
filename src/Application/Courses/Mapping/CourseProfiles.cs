﻿namespace SkorinosGimnazija.Application.Courses.Mapping;

using AutoMapper;
using Domain.Entities.Courses;
using Dtos;

public class CourseProfiles : Profile
{
    public CourseProfiles()
    {
        CreateMap<Course, CourseDto>()
            .ForMember(x => x.StartDate, x => x.MapFrom(c => c.StartDate.ToDateTime(TimeOnly.MinValue)))
            .ForMember(x => x.EndDate, x => x.MapFrom(c => c.EndDate.ToDateTime(TimeOnly.MinValue)));

        CreateMap<CourseEditDto, Course>()
            .ForMember(x => x.StartDate, x => x.MapFrom(c => DateOnly.FromDateTime(c.StartDate)))
            .ForMember(x => x.EndDate, x => x.MapFrom(c => DateOnly.FromDateTime(c.EndDate)));

        CreateMap<CourseCreateDto, Course>()
            .ForMember(x => x.StartDate, x => x.MapFrom(c => DateOnly.FromDateTime(c.StartDate)))
            .ForMember(x => x.EndDate, x => x.MapFrom(c => DateOnly.FromDateTime(c.EndDate)));
    }
}