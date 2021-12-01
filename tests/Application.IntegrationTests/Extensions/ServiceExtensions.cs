namespace SkorinosGimnazija.Application.IntegrationTests.Extensions;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    public static void RemoveService<T>(this ServiceCollection services)
    {
        var descriptors = services.Where(d => d.ServiceType == typeof(T));

        foreach (var serviceDescriptor in descriptors.ToArray())
        {
            services.Remove(serviceDescriptor);
        }
    }
}