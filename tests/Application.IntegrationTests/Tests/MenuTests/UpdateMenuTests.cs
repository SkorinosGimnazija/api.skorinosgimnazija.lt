namespace SkorinosGimnazija.Application.IntegrationTests.Tests.MenuTests;

using Common.Exceptions;
using Domain.Entities;
using FluentAssertions;
using Menus;
using Menus.Dtos;
using Xunit;

[Collection("App")]
public class UpdateMenuTests
{
    private readonly AppFixture _app;

    public UpdateMenuTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task MenuEdit_ShouldThrowValidationException()
    {
        var menu = new MenuEditDto();
        var command = new MenuEdit.Command(menu);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task MenuEdit_ShouldThrowNotFoundException()
    {
        var menu = new MenuEditDto
        {
            Id = 1,
            LanguageId = 1,
            MenuLocationId = 1,
            Slug = "slug",
            Title = "title"
        };

        var command = new MenuEdit.Command(menu);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task MenuEdit_ShouldEditMenu()
    {
        var menu = new Menu
        {
            LanguageId = 1,
            MenuLocationId = 1,
            Slug = "slug",
            Title = "title",
            Path = "/slug"
        };

        await _app.AddAsync(menu);

        var menuEdit = new MenuEditDto
        {
            Id = menu.Id,
            LanguageId = 1,
            MenuLocationId = 1,
            ParentMenuId = null,
            Slug = "slug-1",
            Title = "title 1"
        };

        var command = new MenuEdit.Command(menuEdit);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Menu>(menuEdit.Id);

        actual.Should().NotBeNull();
        actual.Slug.Should().Be(menuEdit.Slug);
        actual.Title.Should().Be(menuEdit.Title);
        actual.Path.Should().Be(menuEdit.Slug);
    }

    [Fact]
    public async Task MenuEdit_ShouldThrowWhenParentIsSelf()
    {
        var menu = new Menu
        {
            LanguageId = 1,
            MenuLocationId = 1,
            Slug = "slug",
            Title = "title",
            Path = "/slug"
        };

        await _app.AddAsync(menu);

        var menuEdit = new MenuEditDto
        {
            Id = menu.Id,
            LanguageId = 1,
            MenuLocationId = 1,
            ParentMenuId = menu.Id,
            Slug = "new-slug",
            Title = "new-title"
        };

        var command = new MenuEdit.Command(menuEdit);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task MenuEdit_ShouldEditChildPaths()
    {
        var rootMenu = new Menu
        {
            LanguageId = 1,
            MenuLocationId = 1,
            Slug = "slug",
            Title = "title",
            Path = "/slug"
        };

        await _app.AddAsync(rootMenu);

        var subMenu = new Menu
        {
            LanguageId = 1,
            MenuLocationId = 1,
            ParentMenuId = rootMenu.Id,
            Slug = "slug1",
            Title = "title1",
            Path = "/slug/slug1"
        };

        await _app.AddAsync(subMenu);

        var child1 = new Menu
        {
            LanguageId = 1,
            MenuLocationId = 1,
            ParentMenuId = subMenu.Id,
            Slug = "slug2",
            Title = "title2",
            Path = "/slug/slug1/slug2"
        };

        await _app.AddAsync(child1);

        var child2 = new Menu
        {
            LanguageId = 1,
            MenuLocationId = 1,
            ParentMenuId = subMenu.Id,
            Slug = "slug3",
            Title = "title3",
            Path = "/slug/slug1/slug3"
        };

        await _app.AddAsync(child2);

        var menuEdit = new MenuEditDto
        {
            Id = subMenu.Id,
            ParentMenuId = null,
            LanguageId = 1,
            MenuLocationId = 1,
            Slug = "new-slug",
            Title = "new-title"
        };

        var command = new MenuEdit.Command(menuEdit);

        await _app.SendAsync(command);

        var actualEdit = await _app.FindAsync<Menu>(menuEdit.Id);
        var actualChild1 = await _app.FindAsync<Menu>(child1.Id);
        var actualChild2 = await _app.FindAsync<Menu>(child2.Id);

        actualEdit.Should().NotBeNull();
        actualEdit.Path.Should().Be("new-slug");

        actualChild1.Should().NotBeNull();
        actualChild1.Path.Should().Be("new-slug/slug2");

        actualChild2.Should().NotBeNull();
        actualChild2.Path.Should().Be("new-slug/slug3");
    }

    [Fact]
    public async Task MenuEdit_ShouldThrowWhenNewParentIsChild()
    {
        var rootMenu = new Menu
        {
            LanguageId = 1,
            MenuLocationId = 1,
            Slug = "slug",
            Title = "title",
            Path = "/slug"
        };

        await _app.AddAsync(rootMenu);

        var subMenu = new Menu
        {
            LanguageId = 1,
            MenuLocationId = 1,
            ParentMenuId = rootMenu.Id,
            Slug = "slug1",
            Title = "title1",
            Path = "/slug/slug1"
        };

        await _app.AddAsync(subMenu);

        var child1 = new Menu
        {
            LanguageId = 1,
            MenuLocationId = 1,
            ParentMenuId = subMenu.Id,
            Slug = "slug2",
            Title = "title2",
            Path = "/slug/slug1/slug2"
        };

        await _app.AddAsync(child1);

        var child2 = new Menu
        {
            LanguageId = 1,
            MenuLocationId = 1,
            ParentMenuId = subMenu.Id,
            Slug = "slug3",
            Title = "title3",
            Path = "/slug/slug1/slug3"
        };

        await _app.AddAsync(child2);

        var menuEdit = new MenuEditDto
        {
            Id = subMenu.Id,
            ParentMenuId = child2.Id,
            LanguageId = 1,
            MenuLocationId = 1,
            Slug = "new-slug",
            Title = "new-title"
        };

        var command = new MenuEdit.Command(menuEdit);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task MenuDelete_ShouldThrowNotFound()
    {
        var command = new MenuDelete.Command(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task MenuDelete_ShouldDeleteMenu()
    {
        var menu = new Menu
        {
            LanguageId = 1,
            MenuLocationId = 1,
            Slug = "slug",
            Title = "title",
            Path = "/slug"
        };

        await _app.AddAsync(menu);

        var command = new MenuDelete.Command(menu.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Menu>(menu.Id);

        actual.Should().BeNull();
    }
}