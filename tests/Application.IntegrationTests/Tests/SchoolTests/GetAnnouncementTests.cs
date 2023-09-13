namespace SkorinosGimnazija.Application.IntegrationTests.Tests.SchoolTests;
using SkorinosGimnazija.Application.Courses;

using SkorinosGimnazija.Domain.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Exceptions;
using Common.Pagination;
using Domain.Entities.School;
using FluentAssertions;
using Xunit;
using SkorinosGimnazija.Application.School;

[Collection("App")] 
public class GetAnnouncementTests
{
    private readonly AppFixture _app;

    public GetAnnouncementTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task AnnouncementList_ShouldListAnnouncements()
    {
        var ann1 = await _app.AddAsync(new Announcement
        {
           Title = "Title1",
           StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)),
           EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5))
        });

        var ann2 = await _app.AddAsync(new Announcement
        {
            Title = "Title2",
            StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
            EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2))
        });

        var command = new AnnouncementList.Query(new());

        var actual = await _app.SendAsync(command);

        actual.Items.Count.Should().Be(2);
        actual.TotalCount.Should().Be(2);
        actual.Items.Select(x => x.Id).Should().Contain(new[] { ann1.Id, ann2.Id });
    }

    [Fact]
    public async Task AnnouncementDetails_ShouldGetAnnouncement_ById()
    {
        var ann1 = await _app.AddAsync(new Announcement
        {
            Title = "Title1",
            StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3)),
            EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5))
        });

        var command = new AnnouncementDetails.Query(ann1.Id);

        var actual = await _app.SendAsync(command);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(ann1.Id);
        actual.Title.Should().Be(ann1.Title);
        actual.StartTime.Should().Be(ann1.StartTime);
        actual.EndTime.Should().Be(ann1.EndTime);
    }


    [Fact]
    public async Task AnnouncementDetails_ShouldGetPublicAnnouncement_ByCurrentDate()
    {
        var ann1 = await _app.AddAsync(new Announcement
        {
            Title = "Title1",
            StartTime = DateOnly.FromDateTime(DateTime.UtcNow),
            EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5))
        });

        var ann2 = await _app.AddAsync(new Announcement
        {
            Title = "Title2",
            StartTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)),
            EndTime = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(3))
        });

        var command = new AnnouncementsPublicList.Query();

        var actual = await _app.SendAsync(command);

        actual.Count.Should().Be(1);
        actual.Select(x => x.Id).Should().Contain(new[] { ann1.Id });
    }

    [Fact]
    public async Task AnnouncementDetails_ShouldThrowEx_WhenInvalidId()
    {
        var command = new AnnouncementDetails.Query(-1);

        await FluentActions.Invoking(() => _app.SendAsync(command))
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}
