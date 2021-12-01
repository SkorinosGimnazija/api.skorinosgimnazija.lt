namespace SkorinosGimnazija.Application.IntegrationTests.Tests.BannerTests;

using Banners;
using Banners.Dtos;
using Common.Exceptions;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

[Collection("App")]
public class UpdateBannerTests
{
    private readonly AppFixture _app;

    public UpdateBannerTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetDatabase();
    }

    [Fact]
    public async Task BannerEdit_ShouldThrowValidationException()
    {
        var entityDto = new BannerEditDto();
        var command = new BannerEdit.Command(entityDto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task BannerEdit_ShouldThrowNotFoundException()
    {
        var entity = new BannerEditDto
        {
            Id = 1,
            LanguageId = 1,
            Url = "slug",
            Title = "title"
        };

        var command = new BannerEdit.Command(entity);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task BannerEdit_ShouldEditBanner()
    {
        var entity = new Banner
        {
            Id = 1,
            LanguageId = 1,
            Url = "slug",
            Title = "title",
            PictureUrl = "url"
        };

        await _app.AddAsync(entity);

        var entityDto = new BannerEditDto
        {
            Id = entity.Id,
            LanguageId = 1,
            Title = "new-title",
            Url = "new-url",
            Picture = new FormFile(null!, 0, 0, null!, null!)
        };

        _app.MediaManagerMock.SetReturnFilesData(new[] { "new-file.jpg" });

        var command = new BannerEdit.Command(entityDto);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Banner>(entityDto.Id);

        actual.Should().NotBeNull();
        actual.Title.Should().Be(entityDto.Title);
        actual.Url.Should().Be(entityDto.Url);
        actual.PictureUrl.Should().Be("new-file.jpg");
    }

    [Fact]
    public async Task BannerDelete_ShouldThrowNotFound()
    {
        var command = new BannerDelete.Command(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task BannerDelete_ShouldDeleteBanner()
    {
        var entity = new Banner
        {
            Id = 1,
            LanguageId = 1,
            Url = "slug",
            Title = "title",
            PictureUrl = "url"
        };

        await _app.AddAsync(entity);

        var command = new BannerDelete.Command(entity.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Banner>(entity.Id);

        actual.Should().BeNull();
    }
}