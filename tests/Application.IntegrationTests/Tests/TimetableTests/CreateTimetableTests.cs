namespace SkorinosGimnazija.Application.IntegrationTests.Tests.TimetableTests;
using FluentAssertions;
using SkorinosGimnazija.Application.BullyJournal;
using SkorinosGimnazija.Application.BullyReports.Dtos;

using SkorinosGimnazija.Domain.Entities.Bullies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Exceptions;
using Domain.Entities.School;
using Domain.Entities.Timetable;
using Timetable;
using Timetable.Dtos;
using Xunit;

[Collection("App")]
public class CreateTimetableTests
{
    private readonly AppFixture _app;

    public CreateTimetableTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task TimetableCreate_ShouldThrowEx_WhenInvalidData()
    {
        var dto = new TimetableCreateDto();
        var command = new TimetableCreate.Command(dto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task TimetableCreate_ShouldCreateTimetable()
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

        var dto = new TimetableCreateDto
        {
            DayId = 1,
            RoomId = room.Id,
            TimeId = time.Id,
            ClassName = "class name"
        };

        var command = new TimetableCreate.Command(dto);

        var created = await _app.SendAsync(command);

        var actual = await _app.FindAsync<Timetable>(created.Id);

        actual.Should().NotBeNull();
        actual.ClassName.Should().Be(dto.ClassName);
        actual.DayId.Should().Be(dto.DayId);
        actual.RoomId.Should().Be(dto.RoomId);
        actual.TimeId.Should().Be(dto.TimeId);
    }

    [Fact]
    public async Task TimetableDetails_ShouldThrowEx_WhenSameRoomTimeDay()
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
            ClassName = "class name"
        });

        var dto = new TimetableCreateDto
        {
            DayId = 1,
            RoomId = room.Id,
            TimeId = time.Id,
            ClassName = "new class name"
        };

        var command = new TimetableCreate.Command(dto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }
}
