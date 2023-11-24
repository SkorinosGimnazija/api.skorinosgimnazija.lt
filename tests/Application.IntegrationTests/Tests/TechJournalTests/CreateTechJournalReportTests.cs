namespace SkorinosGimnazija.Application.IntegrationTests.Tests.TechJournalTests;

using Common.Exceptions;
using Domain.Entities.TechReports;
using FluentAssertions;
using Moq;
using TechJournal;
using TechJournal.Dtos;
using TechJournal.Notifications;
using Xunit;

[Collection("App")]
public class CreateTechJournalReportTests
{
    private readonly AppFixture _app;
    private readonly int _currentUserId;

    public CreateTechJournalReportTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();

        var user = _app.CreateUserAsync().GetAwaiter().GetResult();
        _currentUserId = user.Id;

        _app.CurrentUserMock.SetCurrentUserData(_currentUserId, user.UserName!);
    }

    [Fact]
    public async Task TechJournalReportCreate_ShouldThrowEx_WhenInvalidData()
    {
        var dto = new TechJournalReportCreateDto();
        var command = new TechJournalReportCreate.Command(dto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task TechJournalReportCreate_ShouldCreateRecord_AsCurrentUser()
    {
        var dto = new TechJournalReportCreateDto
        {
            Place = "Place",
            Details = "Details"
        };

        var command = new TechJournalReportCreate.Command(dto);

        var created = await _app.SendAsync(command);

        var actual = await _app.FindAsync<TechJournalReport>(created.Id);

        actual.Should().NotBeNull();
        actual.Place.Should().Be(dto.Place);
        actual.Details.Should().Be(dto.Details);
        actual.ReportDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        actual.IsFixed.Should().Be(null);
        actual.FixDate.Should().Be(null);
        actual.UserId.Should().Be(_currentUserId);

        _app.NotificationPublisherMock.Mock
            .Verify(x => x.Publish(
                    It.Is<TechJournalReportCreatedNotification>(r => r.Report.Id == actual.Id),
                    It.IsAny<CancellationToken>()),
                Times.Once);
    }
}