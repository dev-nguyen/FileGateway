using FileGateway.Application.Abstractions;
using FileGateway.Application.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FileGateway.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IAuthService, AuthService>();
        services.AddMediatR(cfg =>
        {
            // Assembly.GetExecutingAssembly() lấy Assembly của tầng Application.
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });


        return services;
    }
}
