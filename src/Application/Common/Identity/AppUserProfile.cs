namespace SkorinosGimnazija.Application.Common.Identity;

using AutoMapper;
using Domain.Entities.Identity;

public class AppUserProfile : Profile
{
    public AppUserProfile()
    {
        CreateMap<AppUser, UserDto>();
    }
}