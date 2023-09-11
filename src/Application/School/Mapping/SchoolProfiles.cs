namespace SkorinosGimnazija.Application.School.Mapping;

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.School;
using Dtos;

public class SchoolProfiles : Profile
{
    public SchoolProfiles()
    {
        CreateMap<Classroom, ClassroomDto>();
        
        CreateMap<ClassroomCreateDto, Classroom>();

        CreateMap<ClassroomEditDto, Classroom>();

        CreateMap<Classtime, ClasstimeDto>();
        CreateMap<Classtime, ClasstimeSimpleDto>()
            .ForMember(x => x.StartTime, x => x.MapFrom(z => z.StartTime.ToString("H:mm")))
            .ForMember(x => x.EndTime, x => x.MapFrom(z => z.EndTime.ToString("H:mm")));

        CreateMap<ClasstimeCreateDto, Classtime>();
         
        CreateMap<ClasstimeEditDto, Classtime>();

        CreateMap<Classday, ClassdayDto>();
    }
}
