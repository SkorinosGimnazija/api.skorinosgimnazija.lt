namespace SkorinosGimnazija.Application.IntegrationTests.Tests.MenuTests;

using Common.Exceptions;
using Domain.Entities;
using Domain.Entities.CMS;
using FluentAssertions;
using Menus;
using Xunit;

[Collection("App")]
public class GetMenuTests
{
    private readonly AppFixture _app;

    public GetMenuTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task MenuDetails_ShouldThrowNotFoundException()
    {
        var command = new MenuDetails.Query(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task MenuLocationsList_ShouldListMenuLocations()
    {
        var menuLoc1 = await _app.AddAsync(new MenuLocation { Slug = Path.GetRandomFileName(), Name = "name" });
        var menuLoc2 = await _app.AddAsync(new MenuLocation { Slug = Path.GetRandomFileName(), Name = "name" });

        var command = new MenuLocationsList.Query();

        var actual = await _app.SendAsync(command);

        actual.Select(x => x.Slug).Should().Contain(menuLoc1.Slug, menuLoc2.Slug);
    }

    [Fact]
    public async Task MenuDetails_ShouldFindMenu()
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

        var command = new MenuDetails.Query(menu.Id);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
    }

    [Theory]
    [InlineData(1, null, true)]
    [InlineData(0, null, false)]
    [InlineData(0, 1, true)]
    public async Task PublicMenuList_ShouldListPublishedMenusByLanguage(
        int expected, int? langId, bool isPublished)
    {
        var language = await _app.AddAsync(new Language { Slug = Path.GetRandomFileName(), Name = "name" });
        var location = await _app.AddAsync(new MenuLocation { Slug = Path.GetRandomFileName(), Name = "name" });

        var menu1 = new Menu
        {
            LanguageId = langId ?? language.Id,
            MenuLocationId = location.Id,
            IsPublished = isPublished,
            Title = "title",
            Slug = "slug",
            Path = "/slug"
        };

        await _app.AddAsync(menu1);

        var command = new PublicMenuList.Query(language.Slug);

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(expected);
    }

    [Fact]
    public async Task MenuList_ShouldThrowEx_WhenInvalidPagination()
    {
        var command = new MenuList.Query(new() { Items = int.MaxValue, Page = int.MaxValue });

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Theory]
    [InlineData(1, null, null, true)]
    [InlineData(1, null, null, false)]
    [InlineData(1, 1, null, true)]
    [InlineData(1, null, 1, true)]
    [InlineData(1, 1, 1, true)]
    public async Task MenuList_ShouldListAllMenus(
        int expected, int? langId, int? locId, bool isPublished)
    {
        var language = await _app.AddAsync(new Language { Slug = Path.GetRandomFileName(), Name = "name" });
        var location = await _app.AddAsync(new MenuLocation { Slug = Path.GetRandomFileName(), Name = "name" });

        var menu1 = new Menu
        {
            LanguageId = langId ?? language.Id,
            MenuLocationId = locId ?? location.Id,
            IsPublished = isPublished,
            Title = "title",
            Slug = "slug",
            Path = "/slug"
        };

        await _app.AddAsync(menu1);

        var command = new MenuList.Query(new());

        var actual = await _app.SendAsync(command);

        actual.Items.Should().HaveCount(expected);
        actual.TotalCount.Should().Be(expected);
    }

    [Fact]
    public async Task PublicMenuList_ShouldOrderMenus()
    {
        var lang = await _app.AddAsync(new Language { Slug = Path.GetRandomFileName(), Name = "name" });
        var loc = await _app.AddAsync(new MenuLocation { Slug = Path.GetRandomFileName(), Name = "name" });

        var rng = new Random();
        var menuList = new List<Menu>();

        for (var i = 0; i < 5; i++)
        {
            var menu = new Menu
            {
                LanguageId = lang.Id,
                MenuLocationId = loc.Id,
                IsPublished = true,
                Title = "title" + i,
                Slug = "slug" + i,
                Path = "/slug" + i,
                Order = rng.Next(100)
            };

            menuList.Add(menu);
            await _app.AddAsync(menu);
        }

        var command = new PublicMenuList.Query(lang.Slug);

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(5);
        actual.Select(x => x.Id).Should().ContainInOrder(menuList.OrderBy(x => x.Order).Select(x => x.Id));
    }
}