namespace SkorinosGimnazija.Application.IntegrationTests.Tests.PostTests;

using Common.Exceptions;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.FileManagement;
using Microsoft.AspNetCore.Http;
using Posts;
using Posts.Dtos;
using Xunit;

[Collection("App")]
public class CreatePostTests
{
    private readonly AppFixture _app;

    public CreatePostTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetDatabase();
    }

    [Fact]
    public async Task PostCreate_ShouldThrowValidationException()
    {
        var post = new PostCreateDto();
        var command = new PostCreate.Command(post);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task PostCreate_ShouldCreatePost()
    {
        var post = new PostCreateDto
        {
            Slug = "slug",
            Title = "title",
            LanguageId = 1,
            PublishDate = DateTime.UtcNow
        };

        var command = new PostCreate.Command(post);

        var createdPost = await _app.SendAsync(command);

        var actual = await _app.FindAsync<Post>(createdPost.Id);

        actual.Should().NotBeNull();
    }


    [Fact]
    public async Task PostCreate_ShouldThrowValidationEx_WhenDuplicateNewFileNames()
    {
        var postDto = new PostCreateDto
        {
            Title = "new title",
            Slug = "new slug",
            LanguageId = 1,
            PublishDate = DateTime.UtcNow,
            NewFiles = new FormFileCollection
            {
                new FormFile(null!, 0, 0, null!, "FileName.pdf"),
                new FormFile(null!, 0, 0, null!, "FileName.pdf"),
            }
        };

        var command = new PostCreate.Command(postDto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }
}