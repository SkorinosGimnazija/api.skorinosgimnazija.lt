namespace SkorinosGimnazija.Application.School.Mapping;

using AutoMapper;
using Domain.Entities.School;
using Dtos;

public class SchoolProfiles : Profile
{
    public SchoolProfiles()
    {
        CreateMap<Classroom, ClassroomDto>();
        CreateMap<ClassroomCreateDto, Classroom>();
        CreateMap<ClassroomEditDto, Classroom>();

        CreateMap<ClasstimeShortDay, ClasstimeShortDayDto>();
        CreateMap<ClasstimeShortDayCreateDto, ClasstimeShortDay>();
        CreateMap<ClasstimeShortDayEditDto, ClasstimeShortDay>();

        CreateMap<Classtime, ClasstimeDto>();
        CreateMap<ClasstimeDto, ClasstimeSimpleDto>();
        CreateMap<ClasstimeCreateDto, Classtime>();
        CreateMap<ClasstimeEditDto, Classtime>();
        CreateMap<Classtime, ClasstimeSimpleDto>()
            .ForMember(x => x.StartTime, x => x.MapFrom(z => z.StartTime.ToString("H:mm")))
            .ForMember(x => x.EndTime, x => x.MapFrom(z => z.EndTime.ToString("H:mm")));

        CreateMap<Announcement, AnnouncementDto>();
        CreateMap<AnnouncementCreateDto, Announcement>();
        CreateMap<AnnouncementEditDto, Announcement>();

        CreateMap<Classday, ClassdayDto>();
    }
}