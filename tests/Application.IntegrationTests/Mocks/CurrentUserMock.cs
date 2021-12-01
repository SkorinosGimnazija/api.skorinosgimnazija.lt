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
        _mock.Setup(x => x.UserId).Returns(CurrentUserId);
        _mock.Setup(x => x.IsAdmin()).Returns(IsAdmin);

        services.RemoveService<ICurrentUserService>();
        services.AddTransient(_ => _mock.Object);
    }

    public int CurrentUserId { get; set; }

    public bool IsAdmin { get; set; }
}