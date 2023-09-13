namespace SkorinosGimnazija.Application.IntegrationTests.Tests.SchoolTests;
using FluentAssertions;
using SkorinosGimnazija.Application.Courses.Dtos;

using SkorinosGimnazija.Application.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Exceptions;
using Domain.Entities.School;
using School.Dtos;
using Xunit;
using SkorinosGimnazija.Application.School;
using SkorinosGimnazija.Domain.Entities.Courses;

[Collection("App")]
public class CreateAnnouncementTests
{
    private readonly AppFixture _app;

    public CreateAnnouncementTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task AnnouncementCreate_ShouldThrowEx_WhenInvalidData()
    {
        var announcement = new AnnouncementCreateDto();
        var command = new AnnouncementCreate.Command(announcement);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AnnouncementCreate_ShouldCreateAnnouncement()
    {
        var announcement = new AnnouncementCreateDto
        {
            Title = "Title",
            StartTime = DateOnly.FromDateTime(DateTime.UtcNow),
            EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1))
        };

        var command = new AnnouncementCreate.Command(announcement);

        var createdAnnouncement = await _app.SendAsync(command);

        var actual = await _app.FindAsync<Announcement>(createdAnnouncement.Id);

        actual.Should().NotBeNull();
        actual.Title.Should().Be(announcement.Title);
        actual.StartTime.Should().Be(announcement.StartTime);
        actual.EndTime.Should().Be(announcement.EndTime);
    }
}
