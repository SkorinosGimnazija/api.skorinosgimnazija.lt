namespace SkorinosGimnazija.Application.IntegrationTests.Tests.CoursesTests;
using FluentAssertions;
using SkorinosGimnazija.Application.Courses.Dtos;
using SkorinosGimnazija.Application.Courses;

using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Exceptions;
using Xunit;
using SkorinosGimnazija.Application.Menus;
using SkorinosGimnazija.Domain.Entities.Teacher;

[Collection("App")]
public class CreateCoursesTests
{
    private readonly AppFixture _app;
    private readonly int _currentUserId;

    public CreateCoursesTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetDatabase();

        _currentUserId = _app.CreateUserAsync().GetAwaiter().GetResult();
        _app.CurrentUserMock.SetCurrentUserData(_currentUserId);
    }

    [Fact]
    public async Task CourseAdminList_ShouldListAllCoursesByDate()
    {
        var course1 = await _app.AddAsync(new Course
        {
            DurationInHours = 4,
            StartDate = DateOnly.Parse("2021-01-01"),
            EndDate = DateOnly.Parse("2021-01-04"),
            Title = "Course",
            Organizer = "Organizer",
            UserId = _currentUserId
        });

        var course2 = await _app.AddAsync(new Course
        {
            DurationInHours = 4,
            StartDate = DateOnly.Parse("2021-08-01"),
            EndDate = DateOnly.Parse("2021-08-04"),
            Title = "Course",
            Organizer = "Organizer",
            UserId = _currentUserId
        });

        await _app.AddAsync(new Course
        {
            DurationInHours = 4,
            StartDate = DateOnly.Parse("2023-01-01"),
            EndDate = DateOnly.Parse("2023-01-04"),
            Title = "Course",
            Organizer = "Organizer",
            UserId = _currentUserId
        });

        var starDate = DateTime.Parse("2021-01-01");
        var endDate = DateTime.Parse("2021-12-31");

        var command = new CourseAdminList.Query(starDate, endDate);

        var actual = await _app.SendAsync(command);

        actual.Count.Should().Be(2);
        actual.Select(x => x.Id).Should().Contain(new[] { course1.Id, course2.Id });
    }


    [Fact] 
    public async Task CourseCreate_ShouldThrowEx_WhenInvalidData()
    {
        var menu = new CourseCreateDto();
        var command = new CourseCreate.Command(menu);

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
            Organizer = "Organizer",
        };

        var command = new CourseCreate.Command(course);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
        actual.Title.Should().Be(course.Title);
        actual.UserId.Should().Be(_currentUserId);
    }


}
