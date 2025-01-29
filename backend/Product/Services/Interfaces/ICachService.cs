using CSharpFunctionalExtensions;

namespace Product.Services.Interfaces
{
    public interface ICachService
    {
        Task SetData<T>(string key, T data, int experationSecond = 120);
    
        Task<Result<T>> GetData<T>(string key);

        Task RemoveKey(string key);
    }
}
