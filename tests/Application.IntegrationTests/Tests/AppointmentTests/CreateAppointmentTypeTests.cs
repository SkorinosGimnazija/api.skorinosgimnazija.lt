namespace SkorinosGimnazija.Application.IntegrationTests.Tests.AppointmentTests;

using Appointments;
using Appointments.Dtos;
using Common.Exceptions;
using Domain.Entities.Appointments;
using FluentAssertions;
using Xunit;

[Collection("App")]
public class CreateAppointmentTypeTests
{
    private readonly AppFixture _app;

    public CreateAppointmentTypeTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task AppointmentTypeCreate_ShouldThrowEx_WhenInvalidData()
    {
        var dto = new AppointmentTypeCreateDto();
        var command = new AppointmentTypeCreate.Command(dto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AppointmentTypeCreate_ShouldCreateType()
    {
        var dto = new AppointmentTypeCreateDto
        {
            Name = "Type name",
            Slug = "type-slug",
            RegistrationEnd = DateTime.Parse("2022-01-01").ToUniversalTime(),
            DurationInMinutes = 30,
            InvitePrincipal = true,
            IsPublic = true
        };

        var command = new AppointmentTypeCreate.Command(dto);

        var response = await _app.SendAsync(command);

        var actual = await _app.FindAsync<AppointmentType>(response.Id);

        actual.Should().NotBeNull();
        actual.Name.Should().Be(dto.Name);
        actual.Slug.Should().Be(dto.Slug);
        actual.RegistrationEnd.Should().Be(dto.RegistrationEnd);
        actual.DurationInMinutes.Should().Be(dto.DurationInMinutes);
        actual.InvitePrincipal.Should().Be(dto.InvitePrincipal);
        actual.IsPublic.Should().Be(dto.IsPublic);
    }
}