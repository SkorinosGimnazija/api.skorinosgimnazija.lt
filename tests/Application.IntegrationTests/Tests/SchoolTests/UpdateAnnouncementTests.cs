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
public class UpdateAnnouncementTests
{
    private readonly AppFixture _app;

    public UpdateAnnouncementTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task AnnouncementEdit_ShouldThrowEx_WhenInvalidData()
    {
        var entityDto = new AnnouncementEditDto();
        var command = new AnnouncementEdit.Command(entityDto);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task AnnouncementEdit_ShouldThrowEx_WhenInvalidId()
    {
        var entity = new AnnouncementEditDto
        {
            Id = 1,
            Title = "Title2",
            StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2))
        };

        var command = new AnnouncementEdit.Command(entity);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AnnouncementEdit_ShouldEditAnnouncement()
    {
        var ann = await _app.AddAsync(new Announcement
        {
            Title = "Title2",
            StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2))
        });

        var expected = new AnnouncementEditDto
        {
            Id = ann.Id,
            Title = "Title22",
            StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10)),
            EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(20))
        };

        var command = new AnnouncementEdit.Command(expected);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Announcement>(expected.Id);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(expected.Id);
        actual.Title.Should().Be(expected.Title);
        actual.StartTime.Should().Be(expected.StartTime);
        actual.EndTime.Should().Be(expected.EndTime);
    }

    [Fact]
    public async Task AnnouncementDelete_ShouldThrowNotFound()
    {
        var command = new AnnouncementDelete.Command(0);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task AnnouncementDelete_ShouldDeleteAnnouncement()
    {
        var ann = await _app.AddAsync(new Announcement
        {
            Title = "Title2",
            StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2))
        });

        var command = new AnnouncementDelete.Command(ann.Id);

        await _app.SendAsync(command);

        var actual = await _app.FindAsync<Announcement>(ann.Id);

        actual.Should().BeNull();
    }
}
