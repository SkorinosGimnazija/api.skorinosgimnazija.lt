namespace SkorinosGimnazija.Application.IntegrationTests.Tests.SchoolTests;
using SkorinosGimnazija.Application.Courses;

using SkorinosGimnazija.Domain.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Exceptions;
using Domain.Entities.School;
using FluentAssertions;
using Xunit;
using SkorinosGimnazija.Application.School;

[Collection("App")]
public class GetClassroomTests
{
    private readonly AppFixture _app;

    public GetClassroomTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task ClassroomList_ShouldListClassrooms_ByNumber()
    {
        var class1 = await _app.AddAsync(new Classroom
        {
           Number = 2,
           Name = "Name2"
        });

        var class2 = await _app.AddAsync(new Classroom
        {
            Number = 1,
            Name = "Name1"
        });

        var class3 = await _app.AddAsync(new Classroom
        {
            Number = 3,
            Name = "Name3"
        });

        var command = new ClassroomList.Query();

        var actual = await _app.SendAsync(command);

        actual.Count.Should().Be(3);
        actual.Select(x => x.Id).Should().ContainInOrder(class2.Id, class1.Id, class3.Id);
    }

    [Fact]
    public async Task ClassroomDetails_ShouldGetClassroom_ById()
    {
        var class1 = await _app.AddAsync(new Classroom
        {
            Number = 2,
            Name = "Name2"
        });

        var command = new ClassroomDetails.Query(class1.Id);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(class1.Id);
        actual.Number.Should().Be(class1.Number);
        actual.Name.Should().Be(class1.Name);
    }

    [Fact]
    public async Task ClassroomDetails_ShouldThrowEx_WhenInvalidId()
    {
        var command = new ClassroomDetails.Query(-1);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}
