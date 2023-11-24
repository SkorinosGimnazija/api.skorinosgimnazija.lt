namespace SkorinosGimnazija.Application.IntegrationTests.Tests.SchoolTests;

using Common.Exceptions;
using Domain.Entities.School;
using FluentAssertions;
using School;
using School.Dtos;
using Xunit;

[Collection("App")]
public class CreateClassroomTests
{
    private readonly AppFixture _app;

    public CreateClassroomTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task ClassroomCreate_ShouldThrowEx_WhenInvalidData()
    {
        var classroom = new ClassroomCreateDto();
        var command = new ClassroomCreate.Command(classroom);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ClassroomCreate_ShouldCreateClassroom()
    {
        var classroom = new ClassroomCreateDto
        {
            Name = "CName",
            Number = 5
        };

        var command = new ClassroomCreate.Command(classroom);

        var createdClassroom = await _app.SendAsync(command);

        var actual = await _app.FindAsync<Classroom>(createdClassroom.Id);

        actual.Should().NotBeNull();
        actual.Name.Should().Be(classroom.Name);
        actual.Number.Should().Be(classroom.Number);
    }
}