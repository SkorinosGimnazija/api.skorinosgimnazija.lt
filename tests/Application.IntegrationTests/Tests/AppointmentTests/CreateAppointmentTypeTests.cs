namespace SkorinosGimnazija.Application.IntegrationTests.Tests.AppointmentTests;
using FluentAssertions;
using Moq;
using SkorinosGimnazija.Application.BullyReports.Dtos;
using SkorinosGimnazija.Application.BullyReports.Events;
using SkorinosGimnazija.Application.BullyReports;

using SkorinosGimnazija.Domain.Entities.Bullies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appointments.Dtos;
using Domain.Entities.Appointments;
using Xunit;
using SkorinosGimnazija.Application.Common.Exceptions;
using SkorinosGimnazija.Application.Appointments;

[Collection("App")]
public class CreateAppointmentTypeTests
{
    private readonly AppFixture _app;

    public CreateAppointmentTypeTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetDatabase();
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
            Start = DateTime.Parse("2022-01-01"),
            End = DateTime.Parse("2022-01-10"),
            RegistrationEnd = DateTime.Parse("2022-01-01"),
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
        actual.Start.Should().Be(dto.Start);
        actual.End.Should().Be(dto.End);
        actual.RegistrationEnd.Should().Be(dto.RegistrationEnd);
        actual.DurationInMinutes.Should().Be(dto.DurationInMinutes);
        actual.InvitePrincipal.Should().Be(dto.InvitePrincipal);
        actual.IsPublic.Should().Be(dto.IsPublic);
    }


}
