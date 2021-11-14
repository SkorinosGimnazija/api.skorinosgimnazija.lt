namespace Application.Posts.Mapping;

using AutoMapper;
using Domain.CMS;
using Dtos;

public class PostProfiles : Profile
{
    public PostProfiles()
    {
        CreateMap<Post, PostDto>();

        CreateMap<Post, PostDetailsDto>();

        CreateMap<PostEditDto, Post>()
            .ForMember(x => x.Images, x => x.Ignore())
            .ForMember(x => x.Files, x => x.Ignore());

        CreateMap<PostCreateDto, Post>()
            .ForMember(x => x.Images, x => x.Ignore())
            .ForMember(x => x.Files, x => x.Ignore());

        CreateMap<Post, PostIndexDto>()
            .ForMember(x => x.ObjectID, x => x.MapFrom(p => p.Id.ToString()));

        CreateMap<PostPatchDto, Post>()
            .ForMember(x => x.IsFeatured, x => x.Condition(p => p.IsFeatured is not null))
            .ForMember(x => x.IsPublished, x => x.Condition(p => p.IsPublished is not null));
    }
}