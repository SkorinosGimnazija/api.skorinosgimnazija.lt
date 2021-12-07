namespace SkorinosGimnazija.Application.UnitTests;

using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Menus.Dtos;
using Posts.Dtos;
using Posts.Mapping;
using Xunit;

public class ConditionalMappingTests
{
    private readonly IMapper _mapper;

    public ConditionalMappingTests()
    {
        var configuration = new MapperConfiguration(x => { x.AddMaps(typeof(PostProfiles).Assembly); });
        _mapper = configuration.CreateMapper();
    }

    [Fact]
    public void PostEditDtoToPostShouldIgnoreImagesAndFiles()
    {
        var postDto = new PostEditDto
        {
            Images = new[] { "new-image" },
            Files = new[] { "new-file" }
        };

        var post = new Post
        {
            Images = new() { "old-image" },
            Files = new() { "old-file" }
        };

        var actual = _mapper.Map(postDto, post);

        actual.Images.Should().BeEquivalentTo("old-image");
        actual.Files.Should().BeEquivalentTo("old-file");
    }

    [Fact]
    public void PostToPostIndexShouldMapStringObjectId()
    {
        var post = new Post { Id = 1 };

        var actual = _mapper.Map<PostIndexDto>(post);

        actual.ObjectID.Should().Be(post.Id.ToString());
    }

    [Fact]
    public void MenuToMenuIndex_ShouldMapStringObjectId()
    {
        var menu = new Menu { Id = 1 };

        var actual = _mapper.Map<MenuIndexDto>(menu);

        actual.ObjectID.Should().Be(menu.Id.ToString());
    }

    [Fact]
    public void PostPatchToPostShouldSetFeaturedAndPublished()
    {
        var postPatch = new PostPatchDto
        {
            IsFeatured = true,
            IsPublished = true
        };

        var post = new Post
        {
            IsPublished = false,
            IsFeatured = false
        };

        var actual = _mapper.Map(postPatch, post);

        actual.IsFeatured.Should().Be(true);
        actual.IsPublished.Should().Be(true);
    }

    [Fact]
    public void PostPatchToPostShouldIgnoreNullFeaturedAndPublished()
    {
        var postPatch = new PostPatchDto
        {
            IsFeatured = null,
            IsPublished = null
        };

        var post = new Post
        {
            IsFeatured = true,
            IsPublished = true
        };

        var actual = _mapper.Map(postPatch, post);

        actual.IsFeatured.Should().Be(true);
        actual.IsPublished.Should().Be(true);
    }

    [Fact]
    public void MenuEditToMenuShouldSetAndModifyPath()
    {
        var menu = new MenuEditDto { Slug = "slug" };

        var actual = _mapper.Map<Menu>(menu);

        actual.Path.Should().Be("/slug");
    }

    [Fact]
    public void MenuCreateToMenuShouldSetAndModifyPath()
    {
        var menu = new MenuCreateDto { Slug = "slug" };

        var actual = _mapper.Map<Menu>(menu);

        actual.Path.Should().Be("/slug");
    }
}