namespace SkorinosGimnazija.Application.Banners.Mapping;

using AutoMapper;
using Domain.Entities.CMS;
using Dtos;

public class BannerProfiles : Profile
{
    public BannerProfiles()
    {
        CreateMap<Banner, BannerDto>();

        CreateMap<Banner, BannerPublicDto>();

        CreateMap<BannerEditDto, Banner>();

        CreateMap<BannerCreateDto, Banner>();

        CreateMap<Banner, BannerIndexDto>()
            .ForMember(x => x.ObjectID, x => x.MapFrom(p => p.Id.ToString()));
    }
}