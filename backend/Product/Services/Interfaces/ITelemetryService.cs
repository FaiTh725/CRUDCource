using CSharpFunctionalExtensions;

namespace Product.Services.Interfaces
{
    public interface ITelemetryService
    {
        void RecordProductBought(long productId, int count = 1);

        Task<Result<int>> GetProductCountMetric(long productId);

        Task<Result<decimal>> GetUserTransaction(long userId);
    }
}
