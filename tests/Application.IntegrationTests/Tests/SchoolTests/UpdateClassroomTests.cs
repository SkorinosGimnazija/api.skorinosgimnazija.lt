namespace SkorinosGimnazija.Application.IntegrationTests.Tests.SchoolTests;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.School;
using School;
using School.Dtos;
using Xunit;
using SkorinosGimnazija.Application.Common.Exceptions;

[Collection("App")]
public class UpdateClassroomTests
{
    private readonly AppFixture _app;

    public UpdateClassroomTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task ClassroomEdit_ShouldThrowEx_WhenInvalidData()
    {
        var entityDto = new ClassroomEditDto();
        var command = new ClassroomEdit.Command(entityDto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ClassroomEdit_ShouldThrowEx_WhenInvalidId()
    {
        var entity = new ClassroomEditDto
        {
            Id = 1,
            Name = "Name",
            Number = 1
        };

        var command = new ClassroomEdit.Command(entity);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ClassroomEdit_ShouldEditClassroom()
    {
        var classroom = await _app.AddAsync(new Classroom
        {
            Name = "Name1",
            Number = 1
        });

        var expected = new ClassroomEditDto
        {
            Id = classroom.Id,
            Name = "Name2",
            Number = 2
        };

        var command = new ClassroomEdit.Command(expected);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Classroom>(expected.Id);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(expected.Id);
        actual.Name.Should().Be(expected.Name);
        actual.Number.Should().Be(expected.Number);
    }

    [Fact]
    public async Task ClassroomDelete_ShouldThrowNotFound()
    {
        var command = new ClassroomDelete.Command(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task ClassroomDelete_ShouldDeleteClassroom()
    {
        var classroom = await _app.AddAsync(new Classroom
        {
            Name = "Name",
            Number = 1
        });

        var command = new ClassroomDelete.Command(classroom.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Classroom>(classroom.Id);

        actual.Should().BeNull();
    }
}
