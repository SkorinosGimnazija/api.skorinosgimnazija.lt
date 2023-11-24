namespace SkorinosGimnazija.Application.IntegrationTests.Tests.SchoolTests;

using Common.Exceptions;
using Domain.Entities.School;
using FluentAssertions;
using School;
using Xunit;

[Collection("App")]
public class GetClasstimeTests
{
    private readonly AppFixture _app;

    public GetClasstimeTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task ClasstimeList_ShouldListClasstimes_ByNumber()
    {
        var time1 = await _app.AddAsync(new Classtime
        {
            Number = 2
        });

        var time2 = await _app.AddAsync(new Classtime
        {
            Number = 1
        });

        var class3 = await _app.AddAsync(new Classtime
        {
            Number = 3
        });

        var command = new ClasstimeList.Query();

        var actual = await _app.SendAsync(command);

        actual.Count.Should().Be(3);
        actual.Select(x => x.Id).Should().ContainInOrder(time2.Id, time1.Id, class3.Id);
    }

    [Fact]
    public async Task ClasstimeDetails_ShouldGetClasstime_ById()
    {
        var time = await _app.AddAsync(new Classtime
        {
            Number = 2,
            StartTime = TimeOnly.Parse("7:00"),
            EndTime = TimeOnly.Parse("8:00")
        });

        var command = new ClasstimeDetails.Query(time.Id);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(time.Id);
        actual.Number.Should().Be(time.Number);
        actual.StartTime.Should().Be(time.StartTime);
        actual.EndTime.Should().Be(time.EndTime);
    }

    [Fact]
    public async Task ClasstimeDetails_ShouldThrowEx_WhenInvalidId()
    {
        var command = new ClasstimeDetails.Query(-1);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}