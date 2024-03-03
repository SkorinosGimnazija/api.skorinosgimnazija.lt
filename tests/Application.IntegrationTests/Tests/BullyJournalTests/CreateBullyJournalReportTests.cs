namespace SkorinosGimnazija.Application.IntegrationTests.Tests.BullyJournalTests;

using BullyJournal;
using BullyJournal.Dtos;
using Common.Exceptions;
using Domain.Entities.Bullies;
using FluentAssertions;
using Xunit;

[Collection("App")]
public class CreateBullyJournalReportTests
{
    private readonly AppFixture _app;
    private readonly int _currentUserId;

    public CreateBullyJournalReportTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();

        var user = _app.CreateUserAsync().GetAwaiter().GetResult();
        _currentUserId = user.Id;

        _app.CurrentUserMock.SetCurrentUserData(_currentUserId, user.UserName!);
    }

    [Fact]
    public async Task BullyJournalReportCreate_ShouldThrowEx_WhenInvalidData()
    {
        var dto = new BullyJournalReportCreateDto();
        var command = new BullyJournalReportCreate.Command(dto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task BullyJournalReportCreate_ShouldCreateCourse_AsCurrentUser()
    {
        var dto = new BullyJournalReportCreateDto
        {
            BullyInfo = "Name",
            VictimInfo = "Name",
            Actions = "Actions",
            Details = "Details",
            Date = DateTime.Parse("2022-01-01T12:12").ToUniversalTime()
        };

        var command = new BullyJournalReportCreate.Command(dto);

        var created = await _app.SendAsync(command);

        var actual = await _app.FindAsync<BullyJournalReport>(created.Id);

        actual.Should().NotBeNull();
        actual.BullyInfo.Should().Be(dto.BullyInfo);
        actual.VictimInfo.Should().Be(dto.VictimInfo);
        actual.Actions.Should().Be(dto.Actions);
        actual.Details.Should().Be(dto.Details);
        actual.Date.Should().Be(DateOnly.FromDateTime(dto.Date));
        actual.UserId.Should().Be(_currentUserId);
    }
}