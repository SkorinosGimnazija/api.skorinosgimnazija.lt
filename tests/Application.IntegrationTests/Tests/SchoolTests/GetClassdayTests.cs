namespace SkorinosGimnazija.Application.IntegrationTests.Tests.SchoolTests;
using SkorinosGimnazija.Application.School;

using SkorinosGimnazija.Domain.Entities.School;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

[Collection("App")]
public class GetClassdayTests
{
    private readonly AppFixture _app;

    public GetClassdayTests(AppFixture appFixture)
    {
        _app = appFixture;
        _app.ResetData();
    }

    [Fact]
    public async Task ClassdayList_ShouldListClassdays_ByNumber()
    {
        // days are auto seeded
        
        var command = new ClassdayList.Query();

        var actual = await _app.SendAsync(command);

        actual.Count.Should().Be(5);
        actual.Select(x => x.Number).Should().ContainInOrder(1, 2, 3, 4, 5);
    }
}
