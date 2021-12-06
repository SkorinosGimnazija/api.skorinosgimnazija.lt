namespace SkorinosGimnazija.Application.IntegrationTests.Mocks;

using Common.Interfaces;
using Common.Pagination;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class SearchClientMock
{
    private readonly Mock<ISearchClient> _mock = new();

    public SearchClientMock(ServiceCollection services)
    {
        services.RemoveService<ISearchClient>();
        services.AddTransient(_ => _mock.Object);
    }

    public void SetReturnData(ICollection<int> ids)
    {
        _mock
            .Setup(x => x.SearchPostAsync(It.IsAny<string>(), It.IsAny<PaginationDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PaginatedList<int>(ids.ToList(), ids.Count, 0, ids.Count));
    }
}