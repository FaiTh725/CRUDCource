using CSharpFunctionalExtensions;

namespace Authorize.Services.Interfaces
{
    public interface ICacheService
    {
        Task<Result<T>> GetData<T>(string key);

        Task SetData<T>(string key, T data);

        Task<bool> DeleteData(string key);
    }
}
