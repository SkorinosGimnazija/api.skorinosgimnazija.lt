namespace SkorinosGimnazija.Application.IntegrationTests.Tests.MenuTests;

using Common.Exceptions;
using Domain.Entities;
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
        _app.ResetDatabase();
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
    [InlineData(1, null, null, true)]
    [InlineData(0, null, null, false)]
    [InlineData(0, 1, null, true)]
    [InlineData(0, null, 1, true)]
    [InlineData(0, 1, 1, true)]
    public async Task PublicMenuList_ShouldListPublishedMenusByLanguageAndLocation(
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

        var command = new PublicMenuList.Query(language.Slug, location.Slug);

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(expected);
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

        var command = new MenuList.Query();

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(expected);
    }

    [Fact]
    public async Task PublicMenuList_ShouldOrderMenus()
    {
        var lang = await _app.AddAsync(new Language { Slug = Path.GetRandomFileName(), Name = "name" });
        var loc = await _app.AddAsync(new MenuLocation { Slug = Path.GetRandomFileName(), Name = "name" });

        var rng = new Random();

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

            await _app.AddAsync(menu);
        }

        var command = new PublicMenuList.Query(lang.Slug, loc.Slug);

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(5);
        actual.Select(x => x.Order).Should().BeInAscendingOrder();
    }
}