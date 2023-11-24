namespace SkorinosGimnazija.Application.Timetable.Mapping;

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