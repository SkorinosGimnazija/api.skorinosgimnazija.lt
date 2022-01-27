namespace SkorinosGimnazija.Application.IntegrationTests.Tests.CoursesTests;
using SkorinosGimnazija.Application.Languages;

using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Teacher;
using FluentAssertions;
using Xunit;
using SkorinosGimnazija.Application.Banners;
using SkorinosGimnazija.Application.Courses;
using SkorinosGimnazija.Application.Menus;
using SkorinosGimnazija.Application.Common.Exceptions;

[Collection("App")]
public class GetCoursesTests
{
    private readonly AppFixture _app;
    private readonly int _currentUserId;

    public GetCoursesTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();

        var user = _app.CreateUserAsync().GetAwaiter().GetResult();
        _currentUserId = user.Id;

        _app.CurrentUserMock.SetCurrentUserData(_currentUserId, user.UserName);
    }

   

    [Fact]
    public async Task CourseList_ShouldThrowEx_WhenInvalidPagination()
    {
        var command = new CourseList.Query(new() {Page = int.MaxValue, Items = int.MaxValue}) ;

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    } 

    [Fact]
    public async Task CourseStats_ShouldListStats_ByTeacher()
    {
        var teacher1 = await _app.CreateUserAsync();
        var teacher2 = await _app.CreateUserAsync();
        var teacher3 = await _app.CreateUserAsync();
         
        var course1 = await _app.AddAsync(new Course
        {
            DurationInHours = 4,
            StartDate = DateOnly.Parse("2020-01-01"),
            EndDate = DateOnly.Parse("2020-01-04"),
            Title = "Course",
            Organizer = "Organizer",
            UserId = teacher1.Id
        });

        var course2 = await _app.AddAsync(new Course
        {
            DurationInHours = 4,
            StartDate = DateOnly.Parse("2021-08-01"),
            EndDate = DateOnly.Parse("2021-08-04"),
            Title = "Course",
            Organizer = "Organizer",
            UserId = teacher1.Id
        });
         
        var course3 = await _app.AddAsync(new Course
        {
            DurationInHours = 4,
            Price = 20.2f,
            StartDate = DateOnly.Parse("2021-08-01"),
            EndDate = DateOnly.Parse("2021-08-04"),
            IsUseful = true,
            Title = "Course",
            Organizer = "Organizer",
            UserId = teacher2.Id
        });

        var course4 = await _app.AddAsync(new Course
        {
            DurationInHours = 10.5f,
            Price =  10,
            StartDate = DateOnly.Parse("2021-10-01"),
            EndDate = DateOnly.Parse("2021-10-04"),
            IsUseful = true,
            Title = "Course",
            Organizer = "Organizer",
            UserId = teacher2.Id
        });

        var starDate = DateTime.Parse("2021-01-01");
        var endDate = DateTime.Parse("2021-12-31");
         
        var command = new CourseStats.Query(starDate, endDate);

        var actual = await _app.SendAsync(command);

        actual.Count.Should().Be(2);
        actual.Select(x => x.UserId).Should().Contain(new[] { teacher1.Id, teacher2.Id }).And.HaveCount(2);
        actual.Select(x => x.UserDisplayName).Should().BeInAscendingOrder();

        actual.First(x => x.UserId == teacher1.Id).Price.Should().Be(0f);
        actual.First(x => x.UserId == teacher1.Id).Hours.Should().Be(4f);
        actual.First(x => x.UserId == teacher1.Id).Count.Should().Be(1);
        actual.First(x => x.UserId == teacher1.Id).UsefulCount.Should().Be(0);
        actual.First(x => x.UserId == teacher1.Id)
            .LastUpdate.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));

        actual.First(x => x.UserId == teacher2.Id).Price.Should().Be(30.2f);
        actual.First(x => x.UserId == teacher2.Id).Hours.Should().Be(14.5f);
        actual.First(x => x.UserId == teacher2.Id).Count.Should().Be(2);
        actual.First(x => x.UserId == teacher2.Id).UsefulCount.Should().Be(2);
        actual.First(x => x.UserId == teacher2.Id)
            .LastUpdate.Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task CourseAdminList_ShouldListUserCoursesByDate()
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
            StartDate = DateOnly.Parse("2020-01-01"),
            EndDate = DateOnly.Parse("2020-01-04"),
            Title = "Course",
            Organizer = "Organizer",
            UserId = _currentUserId
        });

        var starDate = DateTime.Parse("2021-01-01");
        var endDate = DateTime.Parse("2021-12-31");

        var command = new CourseAdminList.Query(_currentUserId, starDate, endDate);

        var actual = await _app.SendAsync(command);

        actual.Count.Should().Be(2);
        actual.Select(x => x.Id).Should().Contain(new[] { course1.Id, course2.Id });
        actual.Select(x=> x.EndDate).Should().BeInDescendingOrder();
    }

    [Fact]
    public async Task CourseAdminList_ShouldListAllCoursesByDate()
    {
        var teacher1 = await _app.CreateUserAsync();

        var course1 = await _app.AddAsync(new Course
        {
            DurationInHours = 4,
            StartDate = DateOnly.Parse("2021-01-01"),
            EndDate = DateOnly.Parse("2021-01-04"),
            Title = "Course",
            Organizer = "Organizer",
            UserId = teacher1.Id
        });

        var course2 = await _app.AddAsync(new Course
        {
            DurationInHours = 4,
            StartDate = DateOnly.Parse("2021-08-01"),
            EndDate = DateOnly.Parse("2021-08-04"),
            Title = "Course",
            Organizer = "Organizer",
            UserId = teacher1.Id
        });

        await _app.AddAsync(new Course
        {
            DurationInHours = 4,
            StartDate = DateOnly.Parse("2020-01-01"),
            EndDate = DateOnly.Parse("2020-01-04"),
            Title = "Course",
            Organizer = "Organizer",
            UserId = _currentUserId
        });

        var starDate = DateTime.Parse("2021-01-01");
        var endDate = DateTime.Parse("2021-12-31");

        var command = new CourseAdminList.Query(0, starDate, endDate);

        var actual = await _app.SendAsync(command);

        actual.Count.Should().Be(2);
        actual.Select(x => x.Id).Should().Contain(new[] { course1.Id, course2.Id });
        actual.Select(x => x.EndDate).Should().BeInDescendingOrder();
    }


    [Fact]
    public async Task CourseList_ShouldListOwnerCourses()
    { 
        var randomUser = await _app.CreateUserAsync();

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
            UserId = randomUser.Id
        });

        var command = new CourseList.Query(new());

        var actual = await _app.SendAsync(command);

        actual.TotalCount.Should().Be(2);
        actual.Items.Select(x => x.Id).Should().Contain(new []{ course1.Id, course2.Id});
    }
     
    [Fact]
    public async Task CourseDetails_ShouldGetOwnedCourse()
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

        var command = new CourseDetails.Query(course.Id);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(course.Id);
        actual.UserId.Should().Be(_currentUserId);
    }

    [Fact]
    public async Task CourseDetails_ShouldThrowEx_WhenAccessingInvalidCourse()
    { 
        var command = new CourseDetails.Query(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact] 
    public async Task CourseDetails_ShouldThrowEx_WhenAccessingNotOwnedCourse()
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

        var command = new CourseDetails.Query(course.Id);
         
        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }



}
