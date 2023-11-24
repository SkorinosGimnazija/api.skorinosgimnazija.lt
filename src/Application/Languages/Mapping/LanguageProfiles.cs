namespace SkorinosGimnazija.Application.Languages.Mapping;

using AutoMapper;
using Domain.Entities.CMS;
using Dtos;

public class LanguageProfiles : Profile
{
    public LanguageProfiles()
    {
        CreateMap<Language, LanguageDto>();
    }
}