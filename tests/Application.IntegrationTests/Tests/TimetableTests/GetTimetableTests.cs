namespace SkorinosGimnazija.Application.IntegrationTests.Tests.TimetableTests;
using FluentAssertions;
using SkorinosGimnazija.Application.BullyJournal;
using SkorinosGimnazija.Application.Common.Exceptions;

using SkorinosGimnazija.Domain.Entities.Bullies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Timetable;
using Timetable;
using Xunit;
using SkorinosGimnazija.Domain.Entities.School;

[Collection("App")]
public class GetTimetableTests
{
    private readonly AppFixture _app;

    public GetTimetableTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }


    [Fact]
    public async Task TimetableList_ShouldThrowEx_WhenInvalidPagination()
    {
        var command = new TimetableList.Query(new() { Page = int.MaxValue, Items = int.MaxValue });

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact] 
    public async Task TimetableList_ShouldListTimetable()
    {
        var room = await _app.AddAsync(new Classroom
        {
            Name = "Room1",
            Number = 1
        });

        var time = await _app.AddAsync(new Classtime
        {
            Number = 1
        });

        var timetable1 = await _app.AddAsync(new Timetable
        {
            DayId = 1,
            RoomId = room.Id,
            TimeId = time.Id,
            ClassName = "class name 1"
        });

        var timetable2 = await _app.AddAsync(new Timetable
        {
            DayId = 2,
            RoomId = room.Id,
            TimeId = time.Id,
            ClassName = "class name 2"
        });

        var timetable3 = await _app.AddAsync(new Timetable
        {
            DayId = 3,
            RoomId = room.Id,
            TimeId = time.Id,
            ClassName = "class name 3"
        });

        var command = new TimetableList.Query(new() { Items = 10, Page = 0 });

        var actual = await _app.SendAsync(command);

        actual.TotalCount.Should().Be(3);
        actual.Items.Select(x => x.Id).Should().Contain(new[] { timetable1.Id, timetable2.Id, timetable3.Id });
    }

    [Fact]
    public async Task TimetableDetails_ShouldGetTimetable_ById()
    {
        var room = await _app.AddAsync(new Classroom
        {
            Name = "Room1",
            Number = 1
        });

        var time = await _app.AddAsync(new Classtime
        {
            Number = 1
        });

        var timetable = await _app.AddAsync(new Timetable
        {
            DayId = 1,
            RoomId = room.Id,
            TimeId = time.Id,
            ClassName = "class name 1"
        });

        var command = new TimetableDetails.Query(timetable.Id);

        var actual = await _app.SendAsync(command);

        actual.Id.Should().Be(timetable.Id);
        actual.Room.Name.Should().Be(room.Name);
        actual.Time.Number.Should().Be(time.Number);
        actual.ClassName.Should().Be(timetable.ClassName);
    }

    [Fact]
    public async Task TimetableDetails_ShouldThrowEx_WhenAccessingInvalid()
    {
        var command = new TimetableDetails.Query(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}
