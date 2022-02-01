namespace SkorinosGimnazija.Application.IntegrationTests.Tests.BullyReportTests;

using BullyReports;
using Common.Exceptions;
using Domain.Entities.Bullies;
using FluentAssertions;
using Xunit;

[Collection("App")]
public class GetBullyReportsTests
{
    private readonly AppFixture _app;

    public GetBullyReportsTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task BullyReportDetails_ShouldThrowNotFoundException()
    {
        var command = new BullyReportDetails.Query(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task BullyReportDetails_ShouldFindBullyReport()
    {
        var entity = new BullyReport
        {
            BullyInfo = "Bully name",
            VictimInfo = "Victim name",
            Location = "Location",
            Details = "More details",
            Date = DateTime.Parse("2021-01-01 12:00:00").ToUniversalTime()
        };

        await _app.AddAsync(entity);

        var command = new BullyReportDetails.Query(entity.Id);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
        actual.BullyInfo.Should().Be(entity.BullyInfo);
        actual.VictimInfo.Should().Be(entity.VictimInfo);
        actual.Location.Should().Be(entity.Location);
        actual.Details.Should().Be(entity.Details);
        actual.Date.Should().Be(entity.Date);
        actual.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task BullyReportList_ShouldThrowEx_WhenInvalidPagination()
    {
        var command = new BullyReportList.Query(new() { Items = int.MaxValue, Page = int.MaxValue });

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task BullyReportList_ShouldPaginateBullyReports()
    {
        var entity1 = new BullyReport
        {
            BullyInfo = "Bully name",
            VictimInfo = "Victim name",
            Location = "Location",
            Details = "More details",
            Date = DateTime.Parse("2021-01-01 12:00:00").ToUniversalTime()
        };

        var entity2 = new BullyReport
        {
            BullyInfo = "Bully name2",
            VictimInfo = "Victim name2",
            Location = "Location2",
            Details = "More details2",
            Date = DateTime.Parse("2021-01-02 12:00:00").ToUniversalTime()
        };

        await _app.AddAsync(entity1);
        await _app.AddAsync(entity2);

        var command = new BullyReportList.Query(new());

        var actual = await _app.SendAsync(command);

        actual.Items.Should().HaveCount(2);
        actual.Items.Select(x => x.Id).Should().Contain(new[] { entity1.Id, entity2.Id });
        actual.TotalCount.Should().Be(2);
        actual.PageNumber.Should().Be(0);
        actual.TotalPages.Should().Be(1);
    }
}