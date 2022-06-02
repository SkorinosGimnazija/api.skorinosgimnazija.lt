namespace SkorinosGimnazija.Application.IntegrationTests.Tests.AppointmentTests;

using Appointments;
using Appointments.Dtos;
using Common.Exceptions;
using Domain.Entities.Appointments;
using FluentAssertions;
using Moq;
using Xunit;

[Collection("App")]
public class CreateAppointmentTests
{
    private readonly AppFixture _app;
    private readonly string _currentUserName;
    private readonly string _randomUserName;

    public CreateAppointmentTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();

        var user = _app.CreateUserAsync().GetAwaiter().GetResult();
        var randomUser = _app.CreateUserAsync().GetAwaiter().GetResult();

        _randomUserName = randomUser.UserName;
        _currentUserName = user.UserName;
        _app.CurrentUserMock.SetCurrentUserData(user.Id, _currentUserName);
    }

    [Fact]
    public async Task AppointmentCreate_ShouldThrowEx_WhenInvalidData()
    {
        var dto = new AppointmentCreateDto();
        var command = new AppointmentCreate.Command(dto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AppointmentPublicCreate_ShouldThrowEx_WhenInvalidData()
    {
        var dto = new AppointmentPublicCreateDto();
        var command = new AppointmentPublicCreate.Command(dto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AppointmentPublicCreate_ShouldCreateAppointment()
    {
        var date = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.Now.AddDays(1),
            Type = new()
            {
                Name = "Name",
                Slug = "slug",
                RegistrationEnd = DateTime.Now.AddDays(1),
                DurationInMinutes = 30,
                IsPublic = true,
                InvitePrincipal = false
            }
        });

        var dto = new AppointmentPublicCreateDto
        {
            AttendeeEmail = "gmail@gmail.com",
            AttendeeName = "name",
            UserName = _currentUserName,
            DateId = date.Id,
            CaptchaToken = "token"
        };

        _app.EmployeeServiceMock.SetEmployeeData(_currentUserName, "employee@email");

        var command = new AppointmentPublicCreate.Command(dto);

        var response = await _app.SendAsync(command);

        var actual = await _app.FindAsync<Appointment>(response.Id);

        actual.Should().NotBeNull();

        _app.CaptchaServiceMock.Mock
            .Verify(x => x.ValidateAsync(It.Is<string>(token => token == dto.CaptchaToken)),
                Times.Once);

        _app.CalendarServiceMock.Mock
            .Verify(x => x.AddAppointmentAsync(
                    It.Is<string>(z => z == date.Type.Name),
                    It.IsAny<string>(),
                    It.Is<DateTime>(z => z - date.Date < TimeSpan.FromSeconds(5)),
                    It.Is<DateTime>(
                        z => z - date.Date.AddMinutes(date.Type.DurationInMinutes) < TimeSpan.FromSeconds(5)),
                    It.Is<string[]>(z =>
                        z.Length == 2 && z.Contains(dto.AttendeeEmail) && z.Contains("employee@email"))),
                Times.Once);
    }

    [Fact]
    public async Task AppointmentPublicCreate_ShouldThrowEx_WhenInvalidDate()
    {
        var dto = new AppointmentPublicCreateDto
        {
            AttendeeEmail = "gmail@gmail.com",
            AttendeeName = "name",
            UserName = _currentUserName,
            DateId = 1,
            CaptchaToken = "token"
        };

        var command = new AppointmentPublicCreate.Command(dto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AppointmentPublicCreate_ShouldThrowEx_WhenNotGmail()
    {
        var date = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.Now.AddDays(1),
            Type = new()
            {
                Name = "Name",
                Slug = "slug",
                RegistrationEnd = DateTime.Now.AddDays(1),
                DurationInMinutes = 30,
                IsPublic = true,
                InvitePrincipal = true
            }
        });

        _app.EmployeeServiceMock.SetEmployeeData(_currentUserName, "employee@email");

        var dto = new AppointmentPublicCreateDto
        {
            AttendeeEmail = "email@email.com",
            AttendeeName = "name",
            UserName = _currentUserName,
            DateId = date.Id,
            CaptchaToken = "token"
        };

        var command = new AppointmentPublicCreate.Command(dto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AppointmentCreate_ShouldCreateAppointment()
    {
        var date = await _app.AddAsync(new AppointmentDate
        {
            Date = DateTime.Now.AddDays(1),
            Type = new()
            {
                Name = "Name",
                Slug = "slug",
                RegistrationEnd = DateTime.Now.AddDays(1),
                DurationInMinutes = 30,
                IsPublic = false,
                InvitePrincipal = true
            }
        });

        var dto = new AppointmentCreateDto
        {
            UserName = _randomUserName,
            DateId = date.Id
        };

        _app.EmployeeServiceMock.SetEmployeeData(_currentUserName, "atendee@email");

        var command = new AppointmentCreate.Command(dto);

        var response = await _app.SendAsync(command);

        var actual = await _app.FindAsync<Appointment>(response.Id);

        actual.Should().NotBeNull();

        _app.CalendarServiceMock.Mock
            .Verify(x => x.AddAppointmentAsync(
                    It.Is<string>(z => z == date.Type.Name),
                    It.IsAny<string>(),
                    It.Is<DateTime>(z => z - date.Date < TimeSpan.FromSeconds(5)),
                    It.Is<DateTime>(
                        z => z - date.Date.AddMinutes(date.Type.DurationInMinutes) < TimeSpan.FromSeconds(5)),
                    It.Is<string[]>(z =>
                        z.Length == 3 && z.Contains("principal@email") && z.Contains("atendee@email"))),
                Times.Once);
    }
}