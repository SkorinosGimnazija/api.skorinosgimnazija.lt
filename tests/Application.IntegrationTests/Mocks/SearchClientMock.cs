namespace SkorinosGimnazija.Application.IntegrationTests.Mocks;

using Common.Interfaces;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class SearchClientMock
{
    private readonly Mock<ISearchClient> _mock = new();
    private readonly List<int> _mockData = new();

    public SearchClientMock(ServiceCollection services)
    {
        _mock
            .Setup(x => x.SearchPostAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockData);

        services.RemoveService<ISearchClient>();
        services.AddTransient(_ => _mock.Object);
    }

    public void SetReturnData(IEnumerable<int> ids)
    {
        _mockData.AddRange(ids);
    }
}