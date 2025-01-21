using CSharpFunctionalExtensions;
using Microsoft.OpenApi.Writers;
using Product.Domain.Contracts.Repositories;
using Product.Domain.Models;
using Product.Features.Exceptions;
using Product.Helpers.ResponseApi;
using Product.Services.Interfaces;
using System.Diagnostics.Metrics;
using System.Text.Json;

namespace Product.Services.Implementations
{
    // Maybe should create specific telemetry for each entity ???
    public class TelemetryService : ITelemetryService
    {
        private readonly Counter<long> productBoughtCounter;
        //private readonly Counter<double> accountTransactionsCounter;
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        private readonly IServiceProvider serviceProvider;

        public TelemetryService(
            IConfiguration configuration,
            IServiceProvider serviceProvider,
            IHttpClientFactory httpClientFactory)
        {
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;

            var productMeterName = configuration
                .GetValue<string>("Telemetry:ProductsMetricsName") ??
                throw new AplicationConfigurationException(
                    "Configuration file is empty",
                    "Telemetry Section");

            var meter = new Meter(productMeterName);

            productBoughtCounter = meter.CreateCounter<long>(
                "product-sold-count",
                description: "Count sold products");

            //accountTransactionsCounter = meter.CreateCounter<double>(
            //    "user-total-transactions",
            //    unit: "USD",
            //    description: "Total price of user transactions" +
            //    "(total price all bougth products)");

            meter.CreateObservableGauge(
                "user-total-transactions",
                GetUsersTransactions,
                "USD",
                "Total price of user transactions" +
                "(total price all bougth products)");

            meter.CreateObservableGauge(
                "product-average-rating",
                GetProductsRating,
                description: "Average rating of product");

            Console.WriteLine("hui - 123");
            
            httpClient = httpClientFactory.CreateClient("DefaultHttpClient");
        }

        private IEnumerable<Measurement<double>> GetUsersTransactions()
        {
            using var scope = serviceProvider.CreateScope();

            var accountRepository = scope.ServiceProvider
                .GetRequiredService<IAccountRepository>();

            var transactions = accountRepository.GetAccountsWithTotalTransactions();

            foreach (var transaction in transactions)
            {
                yield return new Measurement<double>(
                    transaction.Transactions,
                    KeyValuePair.Create<string, object?>("UserId", transaction.Email));
            }
        }

        private IEnumerable<Measurement<double>> GetProductsRating()
        {
            using var scope = serviceProvider.CreateScope();

            var feedBackRepository = scope.ServiceProvider
                .GetRequiredService<IFeedBackRepository>();

            var ratings = feedBackRepository.GetProductsRating();

            foreach(var rating in ratings)
            {
                yield return new Measurement<double>(
                    rating.Rating,
                    KeyValuePair.Create<string, object?>("ProductId", rating.ProductId));
            }
        }

        public async Task<Result<int>> GetProductCountMetric(long productId)
        {
            try
            {
                var metricsBaseUrl = configuration
                    .GetValue<string>("APIList:PromethesAPI")
                    ?? throw new AplicationConfigurationException(
                        "Configuration api list is null",
                        "ApiListPrometheus");

                var requestUri = $"{metricsBaseUrl}/api/v1/query?query=product_sold_count_total" +
                    $"{{ProductId = \"{productId}\"}}";

                var response = await httpClient.GetAsync(requestUri);

                if(!response.IsSuccessStatusCode)
                {
                    return Result.Failure<int>("Error get metrics(request is invalid)");
                }

                var responseBodyJson = await response.Content.ReadAsStringAsync();

                var responseBody = JsonSerializer.Deserialize<PromQLJsonModel>(responseBodyJson);

                if(responseBody is null)
                {
                    return Result.Failure<int>("Deserialize response error");
                }

                if(responseBody.Data.Result.Length == 0)
                {
                    return Result.Success(0);
                }

                int.TryParse(
                    responseBody.Data.Result[0].Value[1].ToString(), 
                    out int countBoughtProduct);

                return Result.Success(countBoughtProduct);
            }
            catch(AplicationConfigurationException ex)
            {
                throw new AplicationConfigurationException(
                    ex.Message,
                    ex.ConfigurationWithError);
            }
            catch
            {
                return Result.Failure<int>("Error get metrics");
            }
        }

        public void RecordProductBought(long productId, int count = 1)
        {
            productBoughtCounter.Add(
                count, 
                KeyValuePair.Create<string, object?>("ProductId", productId));
        }

        public async Task<Result<decimal>> GetUserTransaction(long userId)
        {
            try
            {
                var metricsBaseUrl = configuration
                    .GetValue<string>("APIList:PromethesAPI")
                    ?? throw new AplicationConfigurationException(
                        "Configuration api list is null",
                        "ApiListPrometheus");

                var requestUri = $"{metricsBaseUrl}/api/v1/query?query=user_total_transactions_USD_total" +
                    $"{{UserId = \"{userId}\"}}";

                var response = await httpClient.GetAsync(requestUri);

                if (!response.IsSuccessStatusCode)
                {
                    return Result.Failure<decimal>("Error get metrics(request is invalid)");
                }

                var responseBodyJson = await response.Content.ReadAsStringAsync();

                var responseBody = JsonSerializer.Deserialize<PromQLJsonModel>(responseBodyJson);

                if (responseBody is null)
                {
                    return Result.Failure<decimal>("Deserialize response error");
                }

                if (responseBody.Data.Result.Length == 0)
                {
                    return Result.Success<decimal>(0);
                }

                decimal.TryParse(responseBody.Data.Result[0].Value[1].ToString(),
                    out decimal totalTransactionsUser);

                return Result.Success(totalTransactionsUser);
            }
            catch(AplicationConfigurationException ex)
            {
                throw new AplicationConfigurationException(
                    ex.Message, 
                    ex.ConfigurationWithError);
            }
            catch
            {
                return Result.Failure<decimal>("Error get metrics user transactions");
            }
        }
    }
}
