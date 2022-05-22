namespace SkorinosGimnazija.Application.Meta.Mapping;

using AutoMapper;
using Domain.Entities.CMS;
using Dtos;

public class MetaProfiles : Profile
{
    public MetaProfiles()
    {
        CreateMap<IGrouping<string, Post>, LocaleMetaDto>()
            .ForMember(x => x.Date, x => x.MapFrom(p => p.Max(z => z.PublishedAt)))
            .ForMember(x => x.Url, x => x.MapFrom(m => m.Key))
            .ForMember(x => x.Ln, x => x.MapFrom(m => m.Key));

        CreateMap<Post, PostMetaDto>()
            .ForMember(x => x.Url, x => x.MapFrom(p => "/" + p.Id + "/" + p.Slug))
            .ForMember(x => x.Date, x => x.MapFrom(p => p.ModifiedAt ?? p.PublishedAt))
            .ForMember(x => x.Ln, x => x.MapFrom(p => p.Language.Slug));

        CreateMap<Menu, MenuMetaDto>()
            .ForMember(x => x.Url, x => x.MapFrom(m => "/" + m.Path))
            .ForMember(x => x.Date, x => x.MapFrom(m => m.LinkedPost!.ModifiedAt ?? m.LinkedPost!.PublishedAt))
            .ForMember(x => x.Ln, x => x.MapFrom(m => m.Language.Slug));
    }
}