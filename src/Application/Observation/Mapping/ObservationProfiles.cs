namespace SkorinosGimnazija.Application.Observation.Mapping;

using AutoMapper;
using Common.Dtos;
using Domain.Entities.Identity;
using Domain.Entities.Observation;
using Dtos;
        
public class ObservationProfiles: Profile
{
    public  ObservationProfiles()
    {
        CreateMap<ObservationType, ObservationTypeDto>();
        CreateMap<ObservationTypeCreateDto, ObservationType>();
        CreateMap<ObservationTypeEditDto, ObservationType>();
        
        CreateMap<ObservationLesson, IdNameDto>();
        CreateMap<ObservationLesson, ObservationLessonDto>();
        CreateMap<ObservationLessonCreateDto, ObservationLesson>();
        CreateMap<ObservationLessonEditDto, ObservationLesson>();
        
        CreateMap<ObservationTarget, IdNameDto>();
        CreateMap<ObservationTarget, ObservationTargetDto>();
        CreateMap<ObservationTargetCreateDto, ObservationTarget>();
        CreateMap<ObservationTargetEditDto, ObservationTarget>();

        CreateMap<StudentObservation, StudentObservationDto>();
        CreateMap<StudentObservationCreateDto, StudentObservation>();
        CreateMap<StudentObservationEditDto, StudentObservation>();
    }
} 