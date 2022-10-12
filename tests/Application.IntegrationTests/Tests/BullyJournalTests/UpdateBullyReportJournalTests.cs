namespace SkorinosGimnazija.Application.IntegrationTests.Tests.BullyJournalTests;
using FluentAssertions;
using SkorinosGimnazija.Application.Courses.Dtos;

using SkorinosGimnazija.Application.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Exceptions;
using Xunit;
using SkorinosGimnazija.Application.BullyReports.Dtos;
using SkorinosGimnazija.Application.BullyJournalReports;
using SkorinosGimnazija.Domain.Entities.Courses;
using SkorinosGimnazija.Domain.Entities.Bullies;
using SkorinosGimnazija.Application.BullyJournal;

[Collection("App")]
public class UpdateBullyReportJournalTests
{
    private readonly AppFixture _app;
    private readonly int _currentUserId;

    public UpdateBullyReportJournalTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();

        var user = _app.CreateUserAsync().GetAwaiter().GetResult();
        _currentUserId = user.Id;

        _app.CurrentUserMock.SetCurrentUserData(_currentUserId, user.UserName);
    }

    [Fact]
    public async Task BullyJournalReportEdit_ShouldThrowEx_WhenInvalidData()
    {
        var entityDto = new BullyJournalReportEditDto();
        var command = new BullyJournalReportEdit.Command(entityDto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task BullyJournalReportEdit_ShouldThrowEx_WhenInvalidId()
    {
        var entity = new BullyJournalReportEditDto
        {
            Id = 1,
            BullyInfo = "Name",
            VictimInfo = "Name",
            Actions = "Actions",
            Details = "Details",
            Date = DateTime.Parse("2022-01-01T12:12").ToUniversalTime()
        };

        var command = new BullyJournalReportEdit.Command(entity);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task BullyJournalReportEdit_ShouldEdit_WhenEditingOwned()
    {
        var report = await _app.AddAsync(new BullyJournalReport
        {
            BullyInfo = "Name",
            VictimInfo = "Name",
            Actions = "Actions",
            Details = "Details",
            Date = DateOnly.Parse("2022-01-01"),
            UserId = _currentUserId
        });

        var expected = new BullyJournalReportEditDto
        {
            Id = report.Id,
            BullyInfo = "Name2",
            VictimInfo = "Name2",
            Actions = "Actions2",
            Details = "Details2",
            Date = DateTime.Parse("2022-01-02T11:11").ToUniversalTime(),
        };

        var command = new BullyJournalReportEdit.Command(expected);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<BullyJournalReport>(report.Id);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(expected.Id);
        actual.BullyInfo.Should().Be(expected.BullyInfo);
        actual.VictimInfo.Should().Be(expected.VictimInfo);
        actual.Actions.Should().Be(expected.Actions);
        actual.Details.Should().Be(expected.Details);
        actual.Date.Should().Be(DateOnly.FromDateTime(expected.Date));
        actual.UserId.Should().Be(_currentUserId);
    }

    [Fact]
    public async Task BullyJournalReportEdit_ShouldThrowEx_WhenEditingNotOwned()
    {
        var owner = await _app.CreateUserAsync();

        var report = await _app.AddAsync(new BullyJournalReport
        {
            BullyInfo = "Name",
            VictimInfo = "Name",
            Actions = "Actions",
            Details = "Details",
            Date = DateOnly.Parse("2022-01-01"),
            UserId = owner.Id
        });

        var dto = new BullyJournalReportEditDto
        {
            Id = report.Id,
            BullyInfo = "Name2",
            VictimInfo = "Name2",
            Actions = "Actions2",
            Details = "Details2",
            Date = DateTime.Parse("2022-01-02T11:11").ToUniversalTime(),
        };

        var command = new BullyJournalReportEdit.Command(dto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task BullyJournalReportDelete_ShouldThrowNotFound()
    {
        var command = new BullyJournalReportDelete.Command(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task BullyJournalReportDelete_ShouldDeleteCourse_WhenOwned()
    {
        var report = await _app.AddAsync(new BullyJournalReport
        {
            BullyInfo = "Name",
            VictimInfo = "Name",
            Actions = "Actions",
            Details = "Details",
            Date = DateOnly.Parse("2022-01-01"),
            UserId = _currentUserId
        });

        var command = new BullyJournalReportDelete.Command(report.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<BullyJournalReport>(report.Id);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task BullyJournalReportDelete_ShouldThrowEx_WhenNotOwned()
    {
        var owner = await _app.CreateUserAsync();

        var report = await _app.AddAsync(new BullyJournalReport
        {
            BullyInfo = "Name",
            VictimInfo = "Name",
            Actions = "Actions",
            Details = "Details",
            Date = DateOnly.Parse("2022-01-01"),
            UserId = owner.Id
        });

        var command = new BullyJournalReportDelete.Command(report.Id);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }
}
