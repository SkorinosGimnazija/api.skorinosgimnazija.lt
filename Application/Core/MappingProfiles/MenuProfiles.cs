namespace Application.Core.MappingProfiles
{
    using AutoMapper;
    using Domain.CMS;
    using Menus.Dtos;

    internal class MenuProfiles : Profile
    {
        public MenuProfiles()
        {
            CreateMap<Menu, Menu>();
            CreateMap<Menu, MenuDto>()
                .ForMember(x => x.ParentMenuId,
                    x => x.MapFrom(m => m.ParentMenu == null ? (int?) null : m.ParentMenu.Id));
        }
    }
}