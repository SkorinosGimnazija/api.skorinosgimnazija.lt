namespace SkorinosGimnazija.Application.IntegrationTests.Tests.BullyReportTests;
using FluentAssertions;
using SkorinosGimnazija.Application.BullyReports.Dtos;
using SkorinosGimnazija.Application.BullyReports;

using SkorinosGimnazija.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banners;
using Domain.Entities.Bullies;
using Xunit;
using SkorinosGimnazija.Domain.Entities;

[Collection("App")]
public class UpdateBullyReportsTests
{
    private readonly AppFixture _app;

    public UpdateBullyReportsTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetDatabase();
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
            Date = DateTime.Parse("2021-01-01 12:00:00").ToUniversalTime(),
        };

        await _app.AddAsync(entity);

        var command = new BullyReportDelete.Command(entity.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<BullyReport>(entity.Id);

        actual.Should().BeNull();
    }


}
