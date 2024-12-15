namespace SkorinosGimnazija.Application.Common.Identity;

using AutoMapper;
using Domain.Entities.Identity;
using Dtos;

public class AppUserProfile : Profile
{
    public AppUserProfile()
    {
        CreateMap<AppUser, UserDto>();
        CreateMap<AppUser, IdNameDto>()
            .ForMember(x => x.Name, x => x.MapFrom(z => z.DisplayName));
    }
}