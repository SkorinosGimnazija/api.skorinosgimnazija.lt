namespace SkorinosGimnazija.Application.IntegrationTests.Tests.AccomplishmentTests;

using Accomplishments;
using Accomplishments.Dtos;
using Common.Exceptions;
using Domain.Entities.Accomplishments;
using Domain.Entities.School;
using FluentAssertions;
using Xunit;

[Collection("App")]
public class CreateAccomplishmentsTests
{
    private readonly AppFixture _app;
    private readonly int _currentUserId;

    public CreateAccomplishmentsTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();

        var user = _app.CreateUserAsync().GetAwaiter().GetResult();
        _currentUserId = user.Id;

        _app.CurrentUserMock.SetCurrentUserData(_currentUserId, user.UserName!);
    }

    [Fact]
    public async Task AccomplishmentCreate_ShouldThrowEx_WhenInvalidData()
    {
        var course = new AccomplishmentCreateDto();
        var command = new AccomplishmentCreate.Command(course);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AccomplishmentCreate_ShouldCreateAccomplishment_AsCurrentUser()
    {
        var classroom = await _app.AddAsync(new Classroom { Name = "C1" });

        var accomplishment = new AccomplishmentCreateDto
        {
            Name = "Name1",
            Date = DateTime.Parse("2021-02-01"),
            ScaleId = 1,
            AdditionalTeachers = new List<AccomplishmentCreateTeacherDto>
            {
                new() { Name = "teacher1" },
                new() { Name = "teacher2" }
            },
            Students = new List<AccomplishmentCreateStudentDto>
            {
                new() { Name = "student1", ClassroomId = classroom.Id, AchievementId = 1 },
                new() { Name = "student2", ClassroomId = classroom.Id, AchievementId = 1 }
            }
        };

        var command = new AccomplishmentCreate.Command(accomplishment);

        var createdAccomplishment = await _app.SendAsync(command);

        var actual = await _app.FindAsync<Accomplishment>(createdAccomplishment.Id);
        var actualTeachersCount = await _app.CountAsync<AccomplishmentTeacher>();
        var actualStudentsCount = await _app.CountAsync<AccomplishmentStudent>();

        actual.Should().NotBeNull();
        actual.Name.Should().Be(accomplishment.Name);
        actual.UserId.Should().Be(_currentUserId);
        actualTeachersCount.Should().Be(accomplishment.AdditionalTeachers.Count);
        actualStudentsCount.Should().Be(accomplishment.Students.Count);
    }
}