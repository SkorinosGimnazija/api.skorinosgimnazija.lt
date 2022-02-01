namespace SkorinosGimnazija.Application.IntegrationTests.Tests.BullyReportsTests;

using BullyReports;
using BullyReports.Dtos;
using BullyReports.Events;
using Common.Exceptions;
using Domain.Entities.Bullies;
using FluentAssertions;
using Moq;
using Xunit;

[Collection("App")]
public class CreateBullyReportsTests
{
    private readonly AppFixture _app;

    public CreateBullyReportsTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task BullyReportPublicCreate_ShouldThrowEx_WhenInvalidData()
    {
        var dto = new BullyReportCreateDto();
        var command = new BullyReportPublicCreate.Command(dto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task BullyReportPublicCreate_ShouldCreateBullyReportAndSendNotification()
    {
        var dto = new BullyReportCreateDto
        {
            BullyInfo = "Bully name",
            VictimInfo = "Victim name",
            Location = "Location",
            Details = "More details",
            Date = DateTime.Parse("2021-01-01 12:00:00").ToUniversalTime(),
            CaptchaToken = "token"
        };

        var command = new BullyReportPublicCreate.Command(dto);

        var response = await _app.SendAsync(command);

        var actual = await _app.FindAsync<BullyReport>(response.Id);

        actual.Should().NotBeNull();
        actual.BullyInfo.Should().Be(dto.BullyInfo);
        actual.VictimInfo.Should().Be(dto.VictimInfo);
        actual.Location.Should().Be(dto.Location);
        actual.Details.Should().Be(dto.Details);
        actual.Date.Should().Be(dto.Date);
        actual.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

        _app.CaptchaServiceMock.Mock
            .Verify(x => x.ValidateAsync(It.Is<string>(token => token == dto.CaptchaToken)),
                Times.Once);

        _app.NotificationPublisherMock.Mock
            .Verify(x => x.Publish(
                    It.Is<BullyReportCreatedNotification>(r => r.Report.Id == actual.Id),
                    It.IsAny<CancellationToken>()),
                Times.Once);
    }
}