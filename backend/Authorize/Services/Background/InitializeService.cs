
using Authorize.Dal;
using Microsoft.EntityFrameworkCore;

namespace Authorize.Services.Background
{
    public class InitializeService : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public InitializeService(
            IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = serviceScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Database.CanConnect() &&
                context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            return Task.CompletedTask;
        }
    }
}
