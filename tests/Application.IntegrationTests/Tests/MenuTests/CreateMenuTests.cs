namespace SkorinosGimnazija.Application.IntegrationTests.Tests.MenuTests;

using Common.Exceptions;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.FileManagement;
using Menus;
using Menus.Dtos;
using Microsoft.AspNetCore.Http;
using Posts;
using Posts.Dtos;
using Xunit;

[Collection("App")]
public class CreateMenuTests
{
    private readonly AppFixture _app;

    public CreateMenuTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetDatabase();
    }


    [Fact]
    public async Task MenuCreate_ShouldThrowValidationException()
    {
        var menu = new MenuCreateDto();
        var command = new MenuCreate.Command(menu);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task MenuCreate_ShouldCreateMenu()
    {
        var menu = new MenuCreateDto
        {
            Slug = "slug",
            Title = "title",
            LanguageId = 1,
            MenuLocationId = 1
        };

        var command = new MenuCreate.Command(menu);

        var createdMenu = await _app.SendAsync(command);

        var actual = await _app.FindAsync<Menu>(createdMenu.Id);

        actual.Should().NotBeNull();
        actual.Slug.Should().Be("slug");
        actual.Path.Should().Be("slug");
    }

    [Fact]
    public async Task MenuCreate_ShouldCreateChildMenuWithCorrectPath()
    {
        var parentMenu = new Menu
        {
            Slug = "parent-slug",
            Title = "parent",
            Path = "/parent-slug",
            LanguageId = 1,
            MenuLocationId = 1
        };

        await _app.AddAsync(parentMenu);

        var childMenu = new MenuCreateDto
        {
            Slug = "child-slug",
            Title = "child",
            LanguageId = 1,
            MenuLocationId = 1,
            ParentMenuId = parentMenu.Id
        };

        var command = new MenuCreate.Command(childMenu);

        var createdMenu = await _app.SendAsync(command);

        var actual = await _app.FindAsync<Menu>(createdMenu.Id);

        actual.Should().NotBeNull();
        actual.ParentMenuId.Should().Be(parentMenu.Id);
        actual.Path.Should().Be("/parent-slug/child-slug");
    }
}