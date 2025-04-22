using Microsoft.Extensions.DependencyInjection;

namespace RO.DevTest.Application.IoC;

/// <summary>
/// Represents the dependency injection configuration for the application layer.
/// </summary>
public static class ApplicationDependencyInjector
{
    /// <summary>
    /// Injects the application layer dependencies into the service collection.
    /// </summary>
    /// <param name="services">The service collection to inject dependencies into.</param>
    /// <returns>The service collection with the application layer dependencies injected.</returns>
    public static IServiceCollection InjectApplicationDependencies(this IServiceCollection services)
    {
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationLayer).Assembly);
        });

        return services;
    }
}