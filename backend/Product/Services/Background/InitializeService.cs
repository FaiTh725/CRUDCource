
using Microsoft.EntityFrameworkCore;
using Product.Dal;

namespace Product.Services.Background
{
    public class InitializeService : BackgroundService
    {
        private readonly IServiceScopeFactory serviceProviderFactory;

        public InitializeService(
            IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceProviderFactory = serviceScopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = serviceProviderFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if(context.Database.CanConnect() && 
                context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            return Task.CompletedTask;
        }
    }
}
