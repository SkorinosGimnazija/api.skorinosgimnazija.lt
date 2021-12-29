﻿namespace SkorinosGimnazija.Application.IntegrationTests.Tests.CoursesTests;
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
        _app.ResetDatabase();

        _currentUserId = _app.CreateUserAsync().GetAwaiter().GetResult();
        _app.CurrentUserMock.SetCurrentUserData(_currentUserId);
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
    public async Task CourseList_ShouldListOwnerCourses()
    { 
        var randomUserId = await _app.CreateUserAsync();

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
            UserId = randomUserId
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
        var ownerId = await _app.CreateUserAsync();
         
        var course = await _app.AddAsync(new Course
        {
            DurationInHours = 4, 
            StartDate = DateOnly.Parse("2021-01-01"),
            EndDate = DateOnly.Parse("2021-01-04"),
            Title = "Course",
            Organizer = "Organizer",
            UserId = ownerId
        });

        var command = new CourseDetails.Query(course.Id);
         
        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }



}