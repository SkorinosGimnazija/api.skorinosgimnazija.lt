namespace SkorinosGimnazija.Application.IntegrationTests.Tests.BannerTests;

using Banners;
using Banners.Dtos;
using Common.Exceptions;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

[Collection("App")]
public class CreateBannerTests
{
    private readonly AppFixture _app;

    public CreateBannerTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetDatabase();
    }

    [Fact]
    public async Task BannerCreate_ShouldThrowValidationException()
    {
        var entityDto = new BannerCreateDto();
        var command = new BannerCreate.Command(entityDto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task BannerCreate_ShouldCreateBanner()
    {
        var entityDto = new BannerCreateDto
        {
            Title = "title",
            Url = "url",
            Picture = new FormFile(null!, 0, 0, null!, null!),
            LanguageId = 1
        };

        _app.MediaManagerMock.SetReturnFilesData(new[] { "random/file/path.jpg" });

        var command = new BannerCreate.Command(entityDto);

        var createdBanner = await _app.SendAsync(command);

        var actual = await _app.FindAsync<Banner>(createdBanner.Id);

        actual.Should().NotBeNull();
        actual.PictureUrl.Should().Be("random/file/path.jpg");
    }
}