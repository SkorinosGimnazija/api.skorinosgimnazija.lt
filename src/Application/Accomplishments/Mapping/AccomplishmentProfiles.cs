namespace SkorinosGimnazija.Application.Accomplishments.Mapping;

using AutoMapper;
using Domain.Entities.Accomplishments;
using Dtos;

public class AccomplishmentProfiles : Profile
{
    public AccomplishmentProfiles()
    {
        CreateMap<AccomplishmentCreateDto, Accomplishment>()
            .ForMember(x => x.AdditionalTeachers,
                x => x.MapFrom(t => t.AdditionalTeachers.Where(z => !string.IsNullOrEmpty(z))))
            .ForMember(x => x.Students,
                x => x.MapFrom(t => t.Students.Where(z => !string.IsNullOrEmpty(z))))
            .ForMember(x => x.Date, x => x.MapFrom(c => DateOnly.FromDateTime(c.Date)));

        CreateMap<AccomplishmentEditDto, Accomplishment>()
            .ForMember(x => x.AdditionalTeachers,
                x => x.MapFrom(t => t.AdditionalTeachers.Where(z => !string.IsNullOrEmpty(z))))
            .ForMember(x => x.Students,
                x => x.MapFrom(t => t.Students.Where(z => !string.IsNullOrEmpty(z))))
            .ForMember(x => x.Date, x => x.MapFrom(c => DateOnly.FromDateTime(c.Date)));

        CreateMap<string, AccomplishmentTeacher>()
            .ForMember(x => x.Name, x => x.MapFrom(s => s));

        CreateMap<string, AccomplishmentStudent>()
            .ForMember(x => x.Name, x => x.MapFrom(s => s));

        CreateMap<Accomplishment, AccomplishmentDto>()
            .ForMember(x => x.TeacherDisplayName, x => x.MapFrom(u => u.User.DisplayName))
            .ForMember(x => x.Scale, x => x.MapFrom(s => s.Scale.Name))
            .ForMember(x => x.Date, x => x.MapFrom(d => d.Date.ToDateTime(TimeOnly.MinValue)));
        
        CreateMap<Accomplishment, AccomplishmentDetailsDto>()
            .ForMember(x => x.Date, x => x.MapFrom(d => d.Date.ToDateTime(TimeOnly.MinValue)));

        CreateMap<AccomplishmentTeacher, AccomplishmentTeacherDto>();

        CreateMap<AccomplishmentStudent, AccomplishmentStudentDto>();

        CreateMap<AccomplishmentScale, AccomplishmentScaleDto>();
    }
}