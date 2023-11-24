namespace SkorinosGimnazija.Application.IntegrationTests.Tests.SchoolTests;

using Common.Exceptions;
using Domain.Entities.School;
using FluentAssertions;
using School;
using School.Dtos;
using Xunit;

[Collection("App")]
public class CreateClasstimeTests
{
    private readonly AppFixture _app;

    public CreateClasstimeTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task ClasstimeCreate_ShouldThrowEx_WhenInvalidData()
    {
        var classtime = new ClasstimeCreateDto();
        var command = new ClasstimeCreate.Command(classtime);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ClasstimeCreate_ShouldCreateClasstime()
    {
        var classtime = new ClasstimeCreateDto
        {
            StartTime = TimeOnly.Parse("7:00"),
            EndTime = TimeOnly.Parse("8:00"),
            Number = 5
        };

        var command = new ClasstimeCreate.Command(classtime);

        var createdClasstime = await _app.SendAsync(command);

        var actual = await _app.FindAsync<Classtime>(createdClasstime.Id);

        actual.Should().NotBeNull();
        actual.Number.Should().Be(classtime.Number);
        actual.StartTime.Should().Be(classtime.StartTime);
        actual.EndTime.Should().Be(classtime.EndTime);
    }
}