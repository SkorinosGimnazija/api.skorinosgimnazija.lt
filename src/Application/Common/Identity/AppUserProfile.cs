namespace SkorinosGimnazija.Application.Common.Identity;

using AutoMapper;
using Domain.Entities.Identity;
using Dtos;

public class AppUserProfile : Profile
{
    public AppUserProfile()
    {
        CreateMap<AppUser, UserDto>();
    }
}