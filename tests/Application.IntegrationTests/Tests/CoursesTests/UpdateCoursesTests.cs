namespace SkorinosGimnazija.Application.IntegrationTests.Tests.CoursesTests;

using Banners;
using Common.Exceptions;
using Courses;
using Courses.Dtos;
using Domain.Entities.Teacher;
using FluentAssertions;
using Xunit;

[Collection("App")]
public class UpdateCoursesTests
{
    private readonly AppFixture _app;
    private readonly int _currentUserId;

    public UpdateCoursesTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();

        var user = _app.CreateUserAsync().GetAwaiter().GetResult();
        _currentUserId = user.Id;

        _app.CurrentUserMock.SetCurrentUserData(_currentUserId, user.UserName);
    }

    [Fact]
    public async Task CourseEdit_ShouldThrowEx_WhenInvalidData()
    {
        var entityDto = new CourseEditDto();
        var command = new CourseEdit.Command(entityDto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task CourseEdit_ShouldThrowEx_WhenInvalidId()
    {
        var entity = new CourseEditDto
        {
            Id = 1,
            DurationInHours = 4,
            StartDate = DateTime.Parse("2021-01-01"),
            EndDate = DateTime.Parse("2021-01-04"),
            Title = "Course",
            Organizer = "Organizer"
        };

        var command = new CourseEdit.Command(entity);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CourseEdit_ShouldEditCourse_WhenEditingOwned()
    {
        var course = await _app.AddAsync(new Course
        {
            DurationInHours = 4,
            StartDate = DateOnly.Parse("2021-01-01"),
            EndDate = DateOnly.Parse("2021-01-04"),
            Title = "Course",
            Organizer = "Organizer",
            UserId = _currentUserId
        });

        var expected = new CourseEditDto
        {
            Id = course.Id,
            DurationInHours = 4,
            StartDate = DateTime.Parse("2021-01-01"),
            EndDate = DateTime.Parse("2021-01-04"),
            Title = "Updated name",
            Organizer = "Organizer"
        };

        var command = new CourseEdit.Command(expected);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Course>(expected.Id);

        actual.Should().NotBeNull();
        actual.Title.Should().Be(expected.Title);
        actual.Id.Should().Be(expected.Id);
        actual.UserId.Should().Be(_currentUserId);
    }

    [Fact]
    public async Task CourseEdit_ShouldThrowEx_WhenEditingNotOwned()
    {
        var owner = await _app.CreateUserAsync();

        var course = await _app.AddAsync(new Course
        {
            DurationInHours = 4,
            StartDate = DateOnly.Parse("2021-01-01"),
            EndDate = DateOnly.Parse("2021-01-04"),
            Title = "Course",
            Organizer = "Organizer",
            UserId = owner.Id
        });

        var dto = new CourseEditDto
        {
            Id = course.Id,
            DurationInHours = 4,
            StartDate = DateTime.Parse("2021-01-01"),
            EndDate = DateTime.Parse("2021-01-04"),
            Title = "Updated name",
            Organizer = "Organizer"
        };

        var command = new CourseEdit.Command(dto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task CourseDelete_ShouldThrowNotFound()
    {
        var command = new CourseDelete.Command(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CourseDelete_ShouldDeleteCourse_WhenOwned()
    {
        var course = await _app.AddAsync(new Course
        {
            DurationInHours = 4,
            StartDate = DateOnly.Parse("2021-01-01"),
            EndDate = DateOnly.Parse("2021-01-04"),
            Title = "Course",
            Organizer = "Organizer",
            UserId = _currentUserId
        });

        var command = new CourseDelete.Command(course.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Course>(course.Id);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task CourseDelete_ShouldThrowEx_WhenNotOwned()
    {
        var owner = await _app.CreateUserAsync();

        var course = await _app.AddAsync(new Course
        {
            DurationInHours = 4,
            StartDate = DateOnly.Parse("2021-01-01"),
            EndDate = DateOnly.Parse("2021-01-04"),
            Title = "Course",
            Organizer = "Organizer",
            UserId = owner.Id
        });

        var command = new CourseDelete.Command(course.Id);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }
}