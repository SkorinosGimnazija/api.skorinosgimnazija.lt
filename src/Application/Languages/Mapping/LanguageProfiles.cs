namespace SkorinosGimnazija.Application.Languages.Mapping;

using AutoMapper;
using Domain.Entities;
using Dtos;

public class LanguageProfiles : Profile
{
    public LanguageProfiles()
    {
        CreateMap<Language, LanguageDto>();
    }
}