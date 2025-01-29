using CSharpFunctionalExtensions;
using Microsoft.Extensions.Caching.Distributed;
using Product.Services.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Product.Services.Implementations
{
    public class CachService : ICachService
    {
        private readonly IDistributedCache cache;
        private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            WriteIndented = true,
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public CachService(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task<Result<T>> GetData<T>(string key)
        {
            var jsonData = await cache.GetStringAsync(key);
        
            if(jsonData is null)
            {
                Console.WriteLine("Deserialize error");
                return Result.Failure<T>("Value is not set");
            }

            var data = JsonSerializer.Deserialize<T>(
                jsonData, serializerOptions);
        
            if(data is null)
            {
                Console.WriteLine("Deserialize error");
                return Result.Failure<T>("Deserialize error");
            }

            return Result.Success(data);
        }

        public async Task RemoveKey(string key)
        {
            await cache.RemoveAsync(key);
        }

        public async Task SetData<T>(string key, T data, int experationSecond = 120)
        {
            var cashingOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(experationSecond)
            };

            var jsonData = JsonSerializer.Serialize(data);

            await cache.SetStringAsync(key, jsonData, cashingOptions);
        }
    }
}
