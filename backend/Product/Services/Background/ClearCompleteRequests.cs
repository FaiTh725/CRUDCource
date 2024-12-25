
using Microsoft.OpenApi.Writers;
using Product.Domain.Contracts.Repositories;

namespace Product.Services.Background
{
    public class ClearCompleteRequests : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public ClearCompleteRequests(
            IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                using var scope = serviceScopeFactory.CreateAsyncScope();

                var logger = scope.ServiceProvider.GetService<ILogger>();
                var requestsRepository = scope.ServiceProvider.GetRequiredService<IChangeRoleRepository>();

                try
                {
                    await requestsRepository.ClearCompleteRequest(48);
                }
                catch
                {
                    logger.LogWarning("Error executed clear request task");
                }

                await Task.Delay(TimeSpan.FromDays(2), stoppingToken);
            }
        }
    }
}
