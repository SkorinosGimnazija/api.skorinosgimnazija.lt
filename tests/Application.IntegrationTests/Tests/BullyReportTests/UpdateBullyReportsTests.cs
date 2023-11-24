namespace SkorinosGimnazija.Application.IntegrationTests.Tests.BullyReportTests;

using BullyReports;
using Common.Exceptions;
using Domain.Entities.Bullies;
using FluentAssertions;
using Xunit;

[Collection("App")]
public class UpdateBullyReportsTests
{
    private readonly AppFixture _app;

    public UpdateBullyReportsTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task BullyReportDelete_ShouldThrowNotFound()
    {
        var command = new BullyReportDelete.Command(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task BullyReportDelete_ShouldDeleteBullyReport()
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

        var command = new BullyReportDelete.Command(entity.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<BullyReport>(entity.Id);

        actual.Should().BeNull();
    }
}