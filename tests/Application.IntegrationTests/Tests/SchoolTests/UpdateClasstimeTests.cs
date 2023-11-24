namespace SkorinosGimnazija.Application.IntegrationTests.Tests.SchoolTests;

using Common.Exceptions;
using Domain.Entities.School;
using FluentAssertions;
using School;
using School.Dtos;
using Xunit;

[Collection("App")]
public class UpdateClasstimeTests
{
    private readonly AppFixture _app;

    public UpdateClasstimeTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task ClasstimeEdit_ShouldThrowEx_WhenInvalidData()
    {
        var entityDto = new ClasstimeEditDto();
        var command = new ClasstimeEdit.Command(entityDto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ClasstimeEdit_ShouldThrowEx_WhenInvalidId()
    {
        var entity = new ClasstimeEditDto
        {
            Id = 1,
            Number = 1,
            StartTime = TimeOnly.Parse("7:00"),
            EndTime = TimeOnly.Parse("8:00")
        };

        var command = new ClasstimeEdit.Command(entity);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ClasstimeEdit_ShouldEditClasstime()
    {
        var classtime = await _app.AddAsync(new Classtime
        {
            Number = 1,
            StartTime = TimeOnly.Parse("7:00"),
            EndTime = TimeOnly.Parse("8:00")
        });

        var expected = new ClasstimeEditDto
        {
            Id = classtime.Id,
            StartTime = TimeOnly.Parse("9:00"),
            EndTime = TimeOnly.Parse("10:00")
        };

        var command = new ClasstimeEdit.Command(expected);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Classtime>(expected.Id);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(expected.Id);
        actual.Number.Should().Be(expected.Number);
        actual.StartTime.Should().Be(expected.StartTime);
        actual.EndTime.Should().Be(expected.EndTime);
    }

    [Fact]
    public async Task ClasstimeDelete_ShouldThrowNotFound()
    {
        var command = new ClasstimeDelete.Command(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ClasstimeDelete_ShouldDeleteClasstime()
    {
        var classtime = await _app.AddAsync(new Classtime
        {
            Number = 1,
            StartTime = TimeOnly.Parse("9:00"),
            EndTime = TimeOnly.Parse("10:00")
        });

        var command = new ClasstimeDelete.Command(classtime.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Classtime>(classtime.Id);

        actual.Should().BeNull();
    }
}