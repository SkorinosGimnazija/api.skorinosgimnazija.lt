namespace SkorinosGimnazija.Application.Posts.Mapping;

using AutoMapper;
using Domain.Entities;
using Domain.Entities.CMS;
using Dtos;

public class PostProfiles : Profile
{
    public PostProfiles()
    {
        CreateMap<Post, PostDto>();

        CreateMap<Post, PostPublicDto>();

        CreateMap<Post, PostDetailsDto>();

        CreateMap<Post, PostPublicDetailsDto>();

        CreateMap<PostEditDto, Post>()
            .ForMember(x => x.Images, x => x.Ignore())
            .ForMember(x => x.Files, x => x.Ignore())
            .ForMember(x => x.FeaturedImage, x => x.Ignore());

        CreateMap<PostCreateDto, Post>();

        CreateMap<Post, PostIndexDto>()
            .ForMember(x => x.ObjectID, x => x.MapFrom(p => p.Id.ToString()));

        CreateMap<PostPatchDto, Post>()
            .ForMember(x => x.IsFeatured, x => x.Condition(p => p.IsFeatured is not null))
            .ForMember(x => x.IsPublished, x => x.Condition(p => p.IsPublished is not null));
    }
}