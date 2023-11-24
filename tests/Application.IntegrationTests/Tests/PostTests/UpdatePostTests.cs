namespace SkorinosGimnazija.Application.IntegrationTests.Tests.PostTests;

using Common.Exceptions;
using Domain.Entities.CMS;
using FluentAssertions;
using Posts;
using Posts.Dtos;
using Xunit;

[Collection("App")]
public class UpdatePostTests
{
    private readonly AppFixture _app;

    public UpdatePostTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task PostEdit_ShouldThrowValidationException()
    {
        var post = new PostEditDto();
        var command = new PostEdit.Command(post);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task PostEdit_ShouldThrowNotFoundException()
    {
        var post = new PostEditDto
        {
            Id = 1,
            Title = "title",
            Slug = "slug",
            LanguageId = 1,
            PublishedAt = DateTime.UtcNow
        };

        var command = new PostEdit.Command(post);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task PostEdit_ShouldEditPost()
    {
        var post = new Post { Slug = "slug", Title = "title" };
        await _app.AddAsync(post);

        var postDto = new PostEditDto
        {
            Id = post.Id,
            Title = "new title",
            Slug = "new slug",
            LanguageId = 1,
            PublishedAt = DateTime.UtcNow
        };

        var command = new PostEdit.Command(postDto);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Post>(postDto.Id);

        actual.Title.Should().Be(postDto.Title);
        actual.Slug.Should().Be(postDto.Slug);
    }

    [Fact]
    public async Task PostPatch_ShouldEditPost()
    {
        var post = new Post
        {
            Slug = "slug",
            Title = "title",
            IsFeatured = false
        };

        var postDto = new PostPatchDto { IsFeatured = true };

        await _app.AddAsync(post);

        var command = new PostPatch.Command(post.Id, postDto);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Post>(post.Id);

        actual.IsFeatured.Should().Be((bool) postDto.IsFeatured);
    }

    [Fact]
    public async Task PostPatch_ShouldThrowNotFound()
    {
        var postDto = new PostPatchDto { IsFeatured = true };

        var command = new PostPatch.Command(0, postDto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeletePost_ShouldThrowNotFound()
    {
        var command = new PostDelete.Command(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task DeletePost_ShouldDeletePost()
    {
        var post = new Post
        {
            Slug = "slug",
            Title = "title"
        };

        await _app.AddAsync(post);

        var command = new PostDelete.Command(post.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Post>(post.Id);

        actual.Should().BeNull();
    }
}