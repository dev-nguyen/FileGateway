using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using FileGateway.Application;
using FileGateway.Application.Abstractions;
using FileGateway.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FileGateway.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, string connection, AwsSettings settings)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));
        services.AddSingleton<IPasswordService, PasswordService>();
        services.AddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFileStorageServiceFactory, FileStorageServiceFactory>();

        services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(
            new BasicAWSCredentials(settings.AccessKey, settings.SecretKey),
            RegionEndpoint.GetBySystemName(settings.Region)));

        services.AddHostedService<DatabaseMigration>();

        return services;
    }
}
