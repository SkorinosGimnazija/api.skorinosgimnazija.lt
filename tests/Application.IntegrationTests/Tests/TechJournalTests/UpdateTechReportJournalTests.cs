namespace SkorinosGimnazija.Application.IntegrationTests.Tests.TechJournalTests;

using Common.Exceptions;
using Domain.Entities.TechReports;
using FluentAssertions;
using TechJournal;
using TechJournal.Dtos;
using Xunit;

[Collection("App")]
public class UpdateTechReportJournalTests
{
    private readonly AppFixture _app;
    private readonly int _currentUserId;

    public UpdateTechReportJournalTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();

        var user = _app.CreateUserAsync().GetAwaiter().GetResult();
        _currentUserId = user.Id;

        _app.CurrentUserMock.SetCurrentUserData(_currentUserId, user.UserName!);
    }

    [Fact]
    public async Task TechJournalReportEdit_ShouldThrowEx_WhenInvalidData()
    {
        var entityDto = new TechJournalReportEditDto();
        var command = new TechJournalReportEdit.Command(entityDto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task TechJournalReportEdit_ShouldThrowEx_WhenInvalidId()
    {
        var entity = new TechJournalReportEditDto
        {
            Id = 1,
            Place = "Place",
            Details = "Details"
        };

        var command = new TechJournalReportEdit.Command(entity);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task TechJournalReportEdit_ShouldEdit_WhenEditingOwned()
    {
        var report = await _app.AddAsync(new TechJournalReport
        {
            Place = "Place",
            Details = "Details",
            ReportDate = DateTime.UtcNow,
            UserId = _currentUserId
        });

        var expected = new TechJournalReportEditDto
        {
            Id = report.Id,
            Place = "Place 2",
            Details = "Details 2"
        };

        var command = new TechJournalReportEdit.Command(expected);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<TechJournalReport>(report.Id);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(expected.Id);
        actual.Place.Should().Be(expected.Place);
        actual.Details.Should().Be(expected.Details);
        actual.ReportDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        actual.UserId.Should().Be(_currentUserId);
    }

    [Fact]
    public async Task TechJournalReportEdit_ShouldNotResetStatus_WhenEditingFixed()
    {
        var report = await _app.AddAsync(new TechJournalReport
        {
            Place = "Place",
            Details = "Details",
            Notes = "Notes",
            FixDate = DateTime.UtcNow,
            ReportDate = DateTime.UtcNow,
            IsFixed = true,
            UserId = _currentUserId
        });

        var expected = new TechJournalReportEditDto
        {
            Id = report.Id,
            Place = "Place 2",
            Details = "Details 2"
        };

        var command = new TechJournalReportEdit.Command(expected);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<TechJournalReport>(report.Id);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(expected.Id);
        actual.Place.Should().Be(expected.Place);
        actual.Details.Should().Be(expected.Details);
        actual.ReportDate.Should().BeCloseTo(report.ReportDate, TimeSpan.FromSeconds(5));
        actual.FixDate.Should().BeCloseTo(report.FixDate!.Value, TimeSpan.FromSeconds(5));
        actual.Notes.Should().Be(report.Notes);
        actual.IsFixed.Should().Be(report.IsFixed);
        actual.UserId.Should().Be(_currentUserId);
    }

    [Fact]
    public async Task TechJournalReportEdit_ShouldThrowEx_WhenEditingNotOwned()
    {
        var owner = await _app.CreateUserAsync();

        var report = await _app.AddAsync(new TechJournalReport
        {
            Place = "Place",
            Details = "Details",
            ReportDate = DateTime.UtcNow,
            UserId = owner.Id
        });

        var dto = new TechJournalReportEditDto
        {
            Id = report.Id,
            Place = "Place 2",
            Details = "Details 2"
        };

        var command = new TechJournalReportEdit.Command(dto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task TechJournalReportDelete_ShouldThrowNotFound()
    {
        var command = new TechJournalReportDelete.Command(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task TechJournalReportDelete_ShouldDeleteCourse_WhenOwned()
    {
        var report = await _app.AddAsync(new TechJournalReport
        {
            Place = "Place",
            Details = "Details",
            ReportDate = DateTime.UtcNow,
            UserId = _currentUserId
        });

        var command = new TechJournalReportDelete.Command(report.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<TechJournalReport>(report.Id);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task TechJournalReportDelete_ShouldThrowEx_WhenNotOwned()
    {
        var owner = await _app.CreateUserAsync();

        var report = await _app.AddAsync(new TechJournalReport
        {
            Place = "Place",
            Details = "Details",
            ReportDate = DateTime.UtcNow,
            UserId = owner.Id
        });

        var command = new TechJournalReportDelete.Command(report.Id);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }
}