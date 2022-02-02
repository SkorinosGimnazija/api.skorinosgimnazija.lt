namespace SkorinosGimnazija.Application.IntegrationTests.Tests.CoursesTests;

using Common.Exceptions;
using Courses;
using Courses.Dtos;
using Domain.Entities.Courses;
using FluentAssertions;
using Xunit;

[Collection("App")]
public class CreateCoursesTests
{
    private readonly AppFixture _app;
    private readonly int _currentUserId;

    public CreateCoursesTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();

        var user = _app.CreateUserAsync().GetAwaiter().GetResult();
        _currentUserId = user.Id;

        _app.CurrentUserMock.SetCurrentUserData(_currentUserId, user.UserName);
    }

    [Fact]
    public async Task CourseCreate_ShouldThrowEx_WhenInvalidData()
    {
        var course = new CourseCreateDto();
        var command = new CourseCreate.Command(course);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task CourseCreate_ShouldCreateCourse_AsCurrentUser()
    {
        var course = new CourseCreateDto
        {
            DurationInHours = 4,
            StartDate = DateTime.Parse("2021-01-01"),
            EndDate = DateTime.Parse("2021-01-04"),
            Title = "Course",
            Organizer = "Organizer"
        };

        var command = new CourseCreate.Command(course);

        var createdCourse = await _app.SendAsync(command);

        var actual = await _app.FindAsync<Course>(createdCourse.Id);

        actual.Should().NotBeNull();
        actual.Title.Should().Be(course.Title);
        actual.UserId.Should().Be(_currentUserId);
    }
}