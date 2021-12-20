namespace SkorinosGimnazija.Application.IntegrationTests.Mocks;

using Common.Interfaces;
using Extensions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class CurrentUserMock
{
    private readonly Mock<ICurrentUserService> _mock = new();

    public CurrentUserMock(ServiceCollection services)
    {
        services.RemoveService<ICurrentUserService>();
        services.AddTransient(_ => _mock.Object);
    }

    public void SetCurrentUserData(int userId)
    {
        _mock.Setup(x => x.UserId).Returns(userId);
        _mock.Setup(x => x.IsResourceOwner(It.Is<int>(id => id == userId))).Returns(true);
        _mock.Setup(x => x.IsOwnerOrAdmin(It.Is<int>(id => id == userId))).Returns(true);
    }
}