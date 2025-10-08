using FileGateway.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FileGateway.Infrastructure;

public class DatabaseMigration : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostEnvironment _env;
    private readonly ILogger<DatabaseMigration> _logger;

    public DatabaseMigration(ILogger<DatabaseMigration> logger, IServiceProvider serviceProvider, IHostEnvironment env)
    {
        _serviceProvider = Ensure.IsNotNull(serviceProvider);
        _env = Ensure.IsNotNull(env);

        _logger = Ensure.IsNotNull(logger);
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            // Just run this in Development environment
            if (_env.IsDevelopment())
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await context.Database.MigrateAsync(stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}
