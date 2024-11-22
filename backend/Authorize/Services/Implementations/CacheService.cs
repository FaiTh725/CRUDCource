using Authorize.Services.Interfaces;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Authorize.Services.Implementations
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache cache;

        public CacheService(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task<bool> DeleteData(string key)
        {
            var data = await cache.GetStringAsync(key);

            if(data == null)
            {
                return false;
            }

            await cache.RemoveAsync(key);

            return true;
        }

        public async Task<Result<T>> GetData<T>(string key)
        {
            var data = await cache.GetStringAsync(key);
        
            if(data == null)
            {
                return Result.Failure<T>("Key is not exist");
            }

            return Result.Success<T>(JsonSerializer.Deserialize<T>(data));
        }

        public async Task SetData<T>(string key, T data)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            await cache.SetStringAsync(key, JsonSerializer.Serialize(data), options);
        }
    }
}
