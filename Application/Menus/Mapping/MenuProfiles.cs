namespace Application.Menus.Mapping;

using AutoMapper;
using Domain.CMS;
using Dtos;

internal class MenuProfiles : Profile
{
    public MenuProfiles()
    {
        CreateMap<MenuLocation, MenuLocationDto>();

        CreateMap<MenuCreateDto, Menu>();

        CreateMap<MenuEditDto, Menu>();

        CreateMap<Menu, MenuDto>();

        
        


    }
}