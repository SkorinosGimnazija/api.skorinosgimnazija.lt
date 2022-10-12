namespace SkorinosGimnazija.Application.IntegrationTests.Tests.BullyJournalTests;
using FluentAssertions;

using SkorinosGimnazija.Application.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BullyJournalReports;
using Common.Exceptions;
using Xunit;
using SkorinosGimnazija.Domain.Entities.Courses;
using SkorinosGimnazija.Domain.Entities.Bullies;
using SkorinosGimnazija.Application.BullyJournal;

[Collection("App")]
public class GetBullyJournalReportTests
{
    private readonly AppFixture _app;

    public GetBullyJournalReportTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task BullyJournalReportList_ShouldThrowEx_WhenInvalidPagination()
    {
        var command = new BullyJournalReportList.Query(new() { Page = int.MaxValue, Items = int.MaxValue });

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task BullyJournalReportList_ShouldListReports_FromAllTeachers()
    {
        var teacher1 = await _app.CreateUserAsync();
        var teacher2 = await _app.CreateUserAsync();

        var report1 = await _app.AddAsync(new BullyJournalReport
        {
            BullyInfo = "Name",
            VictimInfo = "Name",
            Actions = "Actions",
            Details = "Details",
            Date = DateOnly.Parse("2022-01-01"),
            UserId = teacher1.Id
        });
         
        var report2 = await _app.AddAsync(new BullyJournalReport
        {
            BullyInfo = "Name2",
            VictimInfo = "Name2",
            Actions = "Actions2",
            Details = "Details2",
            Date = DateOnly.Parse("2022-01-02"),
            UserId = teacher2.Id
        });

        var command = new BullyJournalReportList.Query(new () { Items = 10, Page = 0 });

        var actual = await _app.SendAsync(command);

        actual.TotalCount.Should().Be(2);
        actual.Items.Select(x => x.UserId).Should().Contain(new[] { teacher1.Id, teacher2.Id });
    }

    [Fact]
    public async Task BullyJournalReportDetails_ShouldGetReport_ById()
    {
        var teacher = await _app.CreateUserAsync();
        var report = await _app.AddAsync(new BullyJournalReport
        {
            BullyInfo = "Name",
            VictimInfo = "Name",
            Actions = "Actions",
            Details = "Details",
            Date = DateOnly.Parse("2022-01-01"),
            UserId = teacher.Id
        });

        var command = new BullyJournalReportDetails.Query(report.Id);

        var actual = await _app.SendAsync(command);

        actual.BullyInfo.Should().Be(report.BullyInfo);
        actual.VictimInfo.Should().Be(report.VictimInfo);
        actual.Actions.Should().Be(report.Actions);
        actual.Details.Should().Be(report.Details);
        actual.Date.Should().Be(report.Date.ToDateTime(TimeOnly.MinValue));
        actual.UserId.Should().Be(report.UserId);
    }

    [Fact]
    public async Task BullyJournalReportDetails_ShouldThrowEx_WhenAccessingInvalidCourse()
    {
        var command = new BullyJournalReportDetails.Query(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}
