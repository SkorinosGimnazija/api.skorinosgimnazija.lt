namespace SkorinosGimnazija.Application.IntegrationTests.Tests.TechJournalTests;
using FluentAssertions;
using SkorinosGimnazija.Application.Courses.Dtos;

using SkorinosGimnazija.Application.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Exceptions;
using Domain.Entities.TechReports;
using Infrastructure.Identity;
using Xunit;
using SkorinosGimnazija.Domain.Entities.Courses;
using SkorinosGimnazija.Domain.Entities.Bullies;
using SkorinosGimnazija.Application.TechJournal;
using TechJournal.Dtos;

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

        _app.CurrentUserMock.SetCurrentUserData(_currentUserId, user.UserName);
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
            Details = "Details",
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
            Details = "Details 2",
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
    public async Task TechJournalReportEdit_ShouldResetStatus_WhenEditingFixed()
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
            Details = "Details 2",
        };

        var command = new TechJournalReportEdit.Command(expected);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<TechJournalReport>(report.Id);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(expected.Id);
        actual.Place.Should().Be(expected.Place);
        actual.Details.Should().Be(expected.Details);
        actual.ReportDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        actual.Notes.Should().Be(null);
        actual.FixDate.Should().Be(null);
        actual.IsFixed.Should().Be(null);
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
    public async Task TechJournalReportPatch_ShouldPatch_WhenTechManager()
    {
        _app.CurrentUserMock.SetCurrentUserTechManager();

        var owner = await _app.CreateUserAsync();

        var report = await _app.AddAsync(new TechJournalReport
        {
            Place = "Place",
            Details = "Details",
            ReportDate = DateTime.UtcNow.AddDays(-1),
            UserId = owner.Id
        });

        var expected = new TechJournalReportPatchDto
        {
            IsFixed = true,
            Notes = "Notes"
        };

        var command = new TechJournalReportPatch.Command(report.Id, expected);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<TechJournalReport>(report.Id);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(report.Id);
        actual.Place.Should().Be(report.Place);
        actual.Details.Should().Be(report.Details);
        actual.ReportDate.Should().BeCloseTo(report.ReportDate, TimeSpan.FromSeconds(5));
        actual.FixDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        actual.Notes.Should().Be(expected.Notes);
        actual.IsFixed.Should().Be(expected.IsFixed);
        actual.UserId.Should().Be(owner.Id);
    }

    [Fact]
    public async Task TechJournalReportPatch_ShouldThrowEx_WhenNotTechManager()
    {
        var report = await _app.AddAsync(new TechJournalReport
        {
            Place = "Place",
            Details = "Details",
            ReportDate = DateTime.UtcNow.AddDays(-1),
            UserId = _currentUserId
        });

        var expected = new TechJournalReportPatchDto
        {
            IsFixed = true,
            Notes = "Notes"
        };

        var command = new TechJournalReportPatch.Command(report.Id, expected);

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
