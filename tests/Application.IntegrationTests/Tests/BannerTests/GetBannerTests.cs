﻿namespace SkorinosGimnazija.Application.IntegrationTests.Tests.BannerTests;

using Banners;
using Common.Exceptions;
using Common.Pagination;
using Domain.Entities;
using FluentAssertions;
using Xunit;

[Collection("App")]
public class GetBannerTests
{
    private readonly AppFixture _app;

    public GetBannerTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetDatabase();
    }

    [Fact]
    public async Task BannerDetails_ShouldThrowNotFoundException()
    {
        var command = new BannerDetails.Query(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task BannerDetails_ShouldFindBanner()
    {
        var entity = new Banner
        {
            LanguageId = 1,
            Url = "slug",
            Title = "title",
            IsPublished = false,
            PictureUrl = "/slug/pic.jpg"
        };

        await _app.AddAsync(entity);

        var command = new BannerDetails.Query(entity.Id);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
    }

    [Fact]
    public async Task BannerList_ShouldPaginateBanners()
    {
        var entity1 = new Banner
        {
            LanguageId = 1,
            Url = "slug",
            Title = "title",
            IsPublished = false,
            PictureUrl = "/slug/pic.jpg"
        };

        var entity2 = new Banner
        {
            LanguageId = 1,
            Url = "slug",
            Title = "title",
            IsPublished = false,
            PictureUrl = "/slug/pic.jpg"
        };

        await _app.AddAsync(entity1);
        await _app.AddAsync(entity2);

        var command = new BannerList.Query(new ());

        var actual = await _app.SendAsync(command);
         
        actual.Items.Should().HaveCount(2);
        actual.Items.Select(x => x.Id).Should().Contain(new[] { entity1.Id, entity1.Id });
        actual.TotalCount.Should().Be(2);
        actual.PageNumber.Should().Be(0);
        actual.TotalPages.Should().Be(1);
    }

    [Theory]
    [InlineData(1, null, true)]
    [InlineData(0, null, false)]
    [InlineData(0, 1, true)]
    [InlineData(0, 1, false)]
    public async Task PublicBannerList_ShouldListPublishedBannersByLanguage(
        int expected, int? langId, bool isPublished)
    {
        var language = await _app.AddAsync(new Language { Slug = Path.GetRandomFileName(), Name = "name" });

        var entity = new Banner
        {
            LanguageId = langId ?? language.Id,
            IsPublished = isPublished,
            Title = "title",
            Url = "slug",
            PictureUrl = "/slug"
        };

        await _app.AddAsync(entity);

        var command = new PublicBannerList.Query(language.Slug);

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(expected);
    }

    [Fact]
    public async Task PublicBannerList_ShouldOrderBanners()
    {
        var lang = await _app.AddAsync(new Language { Slug = Path.GetRandomFileName(), Name = "name" });

        var rng = new Random();

        for (var i = 0; i < 5; i++)
        {
            var entity = new Banner
            {
                LanguageId = lang.Id,
                IsPublished = true,
                Title = "title" + i,
                PictureUrl = "slug" + i,
                Url = "/slug" + i,
                Order = rng.Next(100)
            };

            await _app.AddAsync(entity);
        }

        var command = new PublicBannerList.Query(lang.Slug);

        var actual = await _app.SendAsync(command);

        actual.Should().HaveCount(5);
        actual.Select(x => x.Order).Should().BeInAscendingOrder();
    }
}