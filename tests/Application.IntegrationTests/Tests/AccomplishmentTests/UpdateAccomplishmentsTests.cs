namespace SkorinosGimnazija.Application.IntegrationTests.Tests.AccomplishmentTests;

using Accomplishments;
using Accomplishments.Dtos;
using Common.Exceptions;
using Domain.Entities.Accomplishments;
using Domain.Entities.School;
using FluentAssertions;
using Xunit;

[Collection("App")]
public class UpdateAccomplishmentsTests
{
    private readonly AppFixture _app;
    private readonly int _currentUserId;

    public UpdateAccomplishmentsTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();

        var user = _app.CreateUserAsync().GetAwaiter().GetResult();
        _currentUserId = user.Id;

        _app.CurrentUserMock.SetCurrentUserData(_currentUserId, user.UserName!);
    }

    [Fact]
    public async Task AccomplishmentEdit_ShouldThrowEx_WhenInvalidData()
    {
        var entityDto = new AccomplishmentEditDto();
        var command = new AccomplishmentEdit.Command(entityDto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AccomplishmentEdit_ShouldThrowEx_WhenInvalidId()
    {
        var entity = new AccomplishmentEditDto
        {
            Id = 1,
            Name = "Name1",
            Date = DateTime.Parse("2021-02-01"),
            ScaleId = 1
        };

        var command = new AccomplishmentEdit.Command(entity);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AccomplishmentEdit_ShouldEditAccomplishment_WhenEditingOwned()
    {
        var classroom = await _app.AddAsync(new Classroom { Name = "C1", Number = 1 });

        var accomplishment = await _app.AddAsync(new Accomplishment
        {
            Id = 1,
            Name = "Name1",
            Date = DateOnly.Parse("2021-01-01"),
            ScaleId = 1,
            AdditionalTeachers = new List<AccomplishmentTeacher>
            {
                new() { Name = "teacher1" },
                new() { Name = "teacher2" },
                new() { Name = "teacher3" }
            },
            Students = new List<AccomplishmentStudent>
            {
                new() { Name = "student1", ClassroomId = classroom.Id, AchievementId = 1 },
                new() { Name = "student2", ClassroomId = classroom.Id, AchievementId = 1 },
                new() { Name = "student3", ClassroomId = classroom.Id, AchievementId = 1 }
            },
            UserId = _currentUserId
        });

        var expected = new AccomplishmentEditDto
        {
            Id = accomplishment.Id,
            Name = "Name2",
            Date = DateTime.Parse("2022-01-01T00:00"),
            ScaleId = 2,
            AdditionalTeachers = new List<AccomplishmentCreateTeacherDto>
            {
                new() { Name = "teacher" }
            },
            Students = new List<AccomplishmentCreateStudentDto>
            {
                new() { Name = "student", ClassroomId = classroom.Id, AchievementId = 2 }
            }
        };

        var command = new AccomplishmentEdit.Command(expected);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Accomplishment>(expected.Id);
        var actualStudentsCount = await _app.CountAsync<AccomplishmentStudent>();
        var actualTeachersCount = await _app.CountAsync<AccomplishmentTeacher>();

        actual.Should().NotBeNull();
        actual.Name.Should().Be(expected.Name);
        actual.Date.Should().Be(DateOnly.FromDateTime(expected.Date));
        actual.Id.Should().Be(expected.Id);
        actual.ScaleId.Should().Be(expected.ScaleId);
        actual.UserId.Should().Be(_currentUserId);
        actualStudentsCount.Should().Be(expected.Students.Count);
        actualTeachersCount.Should().Be(expected.AdditionalTeachers.Count);
    }

    [Fact]
    public async Task AccomplishmentEdit_ShouldThrowEx_WhenEditingNotOwned()
    {
        var owner = await _app.CreateUserAsync();

        var accomplishment = await _app.AddAsync(new Accomplishment
        {
            Name = "Name1",
            Date = DateOnly.Parse("2021-01-01"),
            ScaleId = 1,
            UserId = owner.Id
        });

        var dto = new AccomplishmentEditDto
        {
            Id = accomplishment.Id,
            Name = "Name2",
            Date = DateTime.Parse("2022-01-01T00:00"),
            ScaleId = 1
        };

        var command = new AccomplishmentEdit.Command(dto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task AccomplishmentDelete_ShouldThrowNotFound()
    {
        var command = new AccomplishmentDelete.Command(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AccomplishmentDelete_ShouldDeleteAccomplishment_WhenOwned()
    {
        var course = await _app.AddAsync(new Accomplishment
        {
            Name = "Name",
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            ScaleId = 1,
            UserId = _currentUserId
        });

        var command = new AccomplishmentDelete.Command(course.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Accomplishment>(course.Id);

        actual.Should().BeNull();
    }

    [Fact]
    public async Task AccomplishmentDelete_ShouldThrowEx_WhenNotOwned()
    {
        var owner = await _app.CreateUserAsync();

        var course = await _app.AddAsync(new Accomplishment
        {
            Name = "Name",
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            ScaleId = 1,
            UserId = owner.Id
        });

        var command = new AccomplishmentDelete.Command(course.Id);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }
}