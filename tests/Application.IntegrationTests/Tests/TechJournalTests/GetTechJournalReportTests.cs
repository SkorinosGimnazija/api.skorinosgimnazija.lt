namespace SkorinosGimnazija.Application.IntegrationTests.Tests.TechJournalTests;
using FluentAssertions;

using SkorinosGimnazija.Application.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Exceptions;
using Domain.Entities.TechReports;
using Xunit;
using SkorinosGimnazija.Domain.Entities.Courses;
using SkorinosGimnazija.Domain.Entities.Bullies;
using SkorinosGimnazija.Application.TechJournal;

[Collection("App")]
public class GetTechJournalReportTests
{
    private readonly AppFixture _app;

    public GetTechJournalReportTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task TechJournalReportList_ShouldThrowEx_WhenInvalidPagination()
    {
        var command = new TechJournalReportList.Query(new() { Page = int.MaxValue, Items = int.MaxValue });

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task TechJournalReportList_ShouldListReports_FromAllTeachers()
    {
        var teacher1 = await _app.CreateUserAsync();
        var teacher2 = await _app.CreateUserAsync();

        var report1 = await _app.AddAsync(new TechJournalReport
        {
            Place = "Place 1",
            Details = "Details 1",
            UserId = teacher1.Id
        });

        var report2 = await _app.AddAsync(new TechJournalReport
        {
            Place = "Place 2",
            Details = "Details 2",
            UserId = teacher2.Id
        });

        var command = new TechJournalReportList.Query(new() { Items = 10, Page = 0 });

        var actual = await _app.SendAsync(command);

        actual.TotalCount.Should().Be(2);
        actual.Items.Select(x => x.UserId).Should().Contain(new[] { teacher1.Id, teacher2.Id });
    }

    [Fact]
    public async Task TechJournalReportDetails_ShouldGetReport_ById()
    {
        var teacher = await _app.CreateUserAsync();
        var report = await _app.AddAsync(new TechJournalReport
        {
            Place = "Place 1",
            Details = "Details 1",
            ReportDate = DateTime.UtcNow,
            IsFixed = null,
            UserId = teacher.Id,
        });

        var command = new TechJournalReportDetails.Query(report.Id);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
        actual.Place.Should().Be(report.Place);
        actual.Details.Should().Be(report.Details);
        actual.ReportDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        actual.Notes.Should().Be(null);
        actual.FixDate.Should().Be(null);
        actual.IsFixed.Should().Be(null);
    }

    [Fact]
    public async Task TechJournalReportDetails_ShouldGetFixedReport_ById()
    {
        var teacher = await _app.CreateUserAsync();
        var report = await _app.AddAsync(new TechJournalReport
        {
            Place = "Place 1",
            Details = "Details 1",
            ReportDate = DateTime.UtcNow,
            FixDate = DateTime.UtcNow.AddHours(1),
            IsFixed  = false,
            Notes = "Notes",
            UserId = teacher.Id,
        });

        var command = new TechJournalReportDetails.Query(report.Id);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
        actual.Place.Should().Be(report.Place);
        actual.Details.Should().Be(report.Details);
        actual.Notes.Should().Be(report.Notes);
        actual.ReportDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        actual.FixDate.Should().BeCloseTo(DateTime.UtcNow.AddHours(1), TimeSpan.FromSeconds(5));
        actual.IsFixed.Should().Be(false);
    }

    [Fact]
    public async Task TechJournalReportDetails_ShouldThrowEx_WhenAccessingInvalidCourse()
    {
        var command = new TechJournalReportDetails.Query(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}
