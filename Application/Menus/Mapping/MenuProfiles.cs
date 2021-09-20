namespace Application.Menus.Mapping
{
    using AutoMapper;
    using Domain.CMS;
    using Dtos;

    internal class MenuProfiles : Profile
    {
        public MenuProfiles()
        {
            CreateMap<MenuCreateDto, Menu>();
            CreateMap<MenuEditDto, Menu>();

            CreateMap<Menu, MenuDto>()
                .ForMember(x => x.ParentMenuId,
                    x => x.MapFrom(m => m.ParentMenu == null ? (int?) null : m.ParentMenu.Id));
        }
    }
}