namespace SkorinosGimnazija.Application.IntegrationTests.Mocks;

using Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class PublisherMock
{
    public PublisherMock(ServiceCollection services)
    {
        services.RemoveService<IPublisher>();
        services.AddTransient(_ => Mock.Object);
    }

    public Mock<IPublisher> Mock { get; } = new();
}