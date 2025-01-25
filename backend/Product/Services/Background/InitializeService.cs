
using Microsoft.EntityFrameworkCore;
using Product.Dal;

namespace Product.Services.Background
{
    public class InitializeService : BackgroundService
    {
        private readonly AppDbContext context;

        public InitializeService(AppDbContext context)
        {
            this.context = context;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if(context.Database.CanConnect() && 
                context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            return Task.CompletedTask;
        }
    }
}
