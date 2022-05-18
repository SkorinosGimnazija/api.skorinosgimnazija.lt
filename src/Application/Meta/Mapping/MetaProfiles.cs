namespace SkorinosGimnazija.Application.Meta.Mapping;

using AutoMapper;
using Domain.Entities.CMS;
using Dtos;

public class MetaProfiles : Profile
{
    public MetaProfiles()
    {
        CreateMap<Post, PostMetaDto>()
            .ForMember(x => x.Url, x => x.MapFrom(m => "/" + m.Id + "/" + m.Slug))
            .ForMember(x => x.Language, x => x.MapFrom(m => m.Language.Slug));

        CreateMap<Menu, MenuMetaDto>()
            .ForMember(x => x.Url, x => x.MapFrom(m => "/" + m.Path))
            .ForMember(x => x.Language, x => x.MapFrom(m => m.Language.Slug));
    }
}