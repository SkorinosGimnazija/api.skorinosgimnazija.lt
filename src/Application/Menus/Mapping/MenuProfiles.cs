namespace SkorinosGimnazija.Application.Menus.Mapping;

using AutoMapper;
using Domain.Entities.CMS;
using Dtos;

public class MenuProfiles : Profile
{
    public MenuProfiles()
    {
        CreateMap<MenuLocation, MenuLocationDto>();

        CreateMap<Menu, MenuDetailsDto>();

        CreateMap<Menu, MenuPublicDto>();

        CreateMap<Menu, MenuDto>()
            .ForMember(x => x.Position, x => x.MapFrom(m => m.MenuLocation.Slug))
            .ForMember(x => x.Path, x => x.MapFrom(m => "/" + m.Path));

        CreateMap<MenuCreateDto, Menu>()
            .ForMember(x => x.Path, x => x.MapFrom(m => m.Slug));

        CreateMap<MenuEditDto, Menu>()
            .ForMember(x => x.Path, x => x.MapFrom(m => m.Slug));

        CreateMap<Menu, MenuIndexDto>()
            .ForMember(x => x.ObjectID, x => x.MapFrom(p => p.Id.ToString()));
    }
}