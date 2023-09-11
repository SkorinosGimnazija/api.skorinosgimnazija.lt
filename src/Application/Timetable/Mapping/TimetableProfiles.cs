namespace SkorinosGimnazija.Application.Timetable.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Timetable;
using Dtos;

public class TimetableProfiles : Profile
{
    public TimetableProfiles()
    {
        CreateMap<Timetable, TimetableDto>();

        CreateMap<Timetable, TimetableSimpleDto>()
            .ForMember(x => x.ClassRoom, x => x.MapFrom(z => z.Room.Name));

        CreateMap<TimetableCreateDto, Timetable>();
        CreateMap<TimetableEditDto, Timetable>();
    }
}
