namespace SkorinosGimnazija.Application.IntegrationTests.Tests.TimetableTests;
using FluentAssertions;
using SkorinosGimnazija.Application.BullyJournal;
using SkorinosGimnazija.Application.BullyReports.Dtos;
using SkorinosGimnazija.Application.Common.Exceptions;

using SkorinosGimnazija.Domain.Entities.Bullies;
using SkorinosGimnazija.Domain.Entities.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Timetable;
using Timetable;
using Timetable.Dtos;
using Xunit;
[Collection("App")]
public class UpdateTimetableTests
{
    private readonly AppFixture _app;
    
    public UpdateTimetableTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task TimetableEdit_ShouldThrowEx_WhenInvalidData()
    {
        var entityDto = new TimetableEditDto();
        var command = new TimetableEdit.Command(entityDto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task TimetableEdit_ShouldThrowEx_WhenInvalidId()
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

        var entity = new TimetableEditDto
        {
            Id = 1,
            DayId = 1,
            RoomId = room.Id,
            TimeId = time.Id,
            ClassName = "class name"
        };

        var command = new TimetableEdit.Command(entity);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task TimetableEdit_ShouldEdit()
    {
        var room1 = await _app.AddAsync(new Classroom
        {
            Name = "Room1",
            Number = 1
        });

        var room2 = await _app.AddAsync(new Classroom
        {
            Name = "Room2",
            Number = 2
        });

        var time1 = await _app.AddAsync(new Classtime
        {
            Number = 1
        });

        var time2 = await _app.AddAsync(new Classtime
        {
            Number = 2
        });

        var timetable = await _app.AddAsync(new Timetable
        {
            DayId = 1,
            RoomId = room1.Id,
            TimeId = time1.Id,
            ClassName = "class name 1"
        });

        var expected = new TimetableEditDto
        {
            Id = timetable.Id,
            DayId = 2,
            RoomId = room2.Id,
            TimeId = time2.Id,
            ClassName = "class name 2"
        };

        var command = new TimetableEdit.Command(expected);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Timetable>(expected.Id);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(expected.Id);
        actual.DayId.Should().Be(expected.DayId);
        actual.RoomId.Should().Be(expected.RoomId);
        actual.TimeId.Should().Be(expected.TimeId);
        actual.ClassName.Should().Be(expected.ClassName);
    }

    [Fact]
    public async Task TimetableDelete_ShouldThrowNotFound()
    {
        var command = new TimetableDelete.Command(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task TimetableDelete_ShouldDelete()
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

        var command = new TimetableDelete.Command(timetable.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Timetable>(timetable.Id);

        actual.Should().BeNull();
    }
}
