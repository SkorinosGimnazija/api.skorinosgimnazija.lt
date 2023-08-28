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

            //.ForMember(x => x.ClassRoom, x => x.MapFrom(z => z.Room.Name))
            //.ForMember(x => x.ClassNumber, x => x.MapFrom(z => z.Time.Number))
            //.ForMember(x => x.DayName, x => x.MapFrom(z => z.Day.Name));

        CreateMap<TimetableCreateDto, Timetable>();
        CreateMap<TimetableEditDto, Timetable>();
    }
}
