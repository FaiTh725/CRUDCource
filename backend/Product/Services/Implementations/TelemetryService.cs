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

            meter.CreateObservableGauge(
                "product-ratings",
                GetProductExtentionRating,
                description: "product feedbacks count");
            
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

        private IEnumerable<Measurement<int>> GetProductExtentionRating()
        {
            using var scope = serviceProvider.CreateScope();

            var feedBackRepository = scope.ServiceProvider
                .GetRequiredService<IFeedBackRepository>();

            var ratings = feedBackRepository.GetProductsRating();

            foreach(var rating in ratings)
            {
                foreach(var ratingUnit in rating.PartRatingCount)
                {
                    yield return new Measurement<int>(ratingUnit.Value, 
                        KeyValuePair.Create<string, object?>("Rating", ratingUnit.Key),
                        KeyValuePair.Create<string, object?>("ProductRatingId", rating.ProductId)); 
                }
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
                    rating.GeneralRating,
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

        public async Task<Result<double>> GetProductGeneralRating(long productId)
        {
            try
            {
                var requestBaseUrl = configuration.GetValue<string>("APIList:PromethesAPI") ??
                    throw new AplicationConfigurationException(
                        "Configuration api list is null",
                        "ApiListPrometheus");

                var requestUrl = $"{requestBaseUrl}/api/v1/query?query=" +
                    $"product_average_rating{{ProductId = \"{productId}\"}}";

                var response = await httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return Result.Failure<double>("Error send inner api request");
                }

                var responseJson = await response.Content.ReadAsStringAsync();

                if (responseJson is null)
                {
                    return Result.Failure<double>("Error get response from inner api");
                }

                var responseData = JsonSerializer.Deserialize<PromQLAverageRating>(responseJson);

                if (responseData is null)
                {
                    return Result.Failure<double>("Error Deserialize Response");
                }

                if (responseData.Status != "success")
                {
                    return Result.Failure<double>("Error PromQL request path");
                }

                if (responseData.Data.Result.Length == 0)
                {
                    return Result.Success<double>(-1);
                }

                double.TryParse(
                    responseData.Data.Result[0].Value[1].ToString(), 
                    out double averageRatin);
                
                
                return Result.Success(averageRatin);
            }
            catch (AplicationConfigurationException ex)
            {
                throw new AplicationConfigurationException(
                    ex.Message,
                    ex.ConfigurationWithError);
            }
            catch
            {
                return Result.Failure<double>("Error Get Metrics " +
                    "Product Averate Rating");
            }
        }

        public async Task<Result<List<KeyValuePair<int, int>>>> GetProductExtentionRating(long productId)
        {
            try
            {
                var requestBaseUrl = configuration.GetValue<string>("APIList:PromethesAPI") ??
                    throw new AplicationConfigurationException(
                        "Configuration api list is null",
                        "ApiListPrometheus");

                var requestUrl = $"{requestBaseUrl}/api/v1/query?query=" +
                    $"product_ratings{{ProductRatingId = \"{productId}\"}}";


                var response = await httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return Result.Failure<List<KeyValuePair<int, int>>>(
                        "Error send inner api request");
                }

                var responseJson = await response.Content.ReadAsStringAsync();

                if (responseJson is null)
                {
                    return Result.Failure<List<KeyValuePair<int, int>>>(
                        "Error get response from inner api");
                }

                var responseData = JsonSerializer.Deserialize<PromQLStatRating>(responseJson);

                if (responseData is null)
                {
                    return Result.Failure<List<KeyValuePair<int, int>>>(
                        "Error Deserialize Response");
                }

                if (responseData.Status != "success")
                {
                    return Result.Failure<List<KeyValuePair<int, int>>>(
                        "Error PromQL request path");
                }

                var ratings = new List<KeyValuePair<int, int>>();

                foreach (var stat in responseData.Data.Result)
                {
                    var statRating = stat.Metric;

                    int.TryParse(statRating.Rating, out int statKey);
                    int.TryParse(stat.Value[1].ToString(), out int statValue);

                    ratings.Add(KeyValuePair.Create(statKey, statValue));
                }

                return Result.Success(ratings);
            }
            catch (AplicationConfigurationException ex)
            {
                throw new AplicationConfigurationException(
                    ex.Message,
                    ex.ConfigurationWithError);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Result.Failure<List<KeyValuePair<int, int>>>(
                    "Error Get Metrics " +
                    "Proudct Extention Rating");
            }
        }
    }
}
