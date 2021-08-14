﻿namespace Application.Core.MappingProfiles
{
using Application.Posts.Dtos;
    using AutoMapper;
    using Domain.CMS;
    using Menus.Dtos;

    internal class MenuProfiles : Profile
    {
        public MenuProfiles()
        {
            CreateMap<Menu, Menu>();
            CreateMap<MenuCreateDto, Menu>();
            CreateMap<MenuEditDto, Menu>();
            CreateMap<Menu, PublicMenuDto>()
                .ForMember(x => x.ParentMenuId,
                    x => x.MapFrom(m => m.ParentMenu == null ? (int?) null : m.ParentMenu.Id));
        }
    }
}