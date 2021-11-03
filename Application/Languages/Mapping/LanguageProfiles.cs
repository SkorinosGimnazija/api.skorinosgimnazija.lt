namespace Application.Languages.Mapping;

using AutoMapper;
using Domain.CMS;
using Dtos;

internal class LanguageProfiles : Profile
{
    public LanguageProfiles()
    {
        CreateMap<Language, LanguageDto>();
    }
}