using CSharpFunctionalExtensions;

namespace Product.Services.Interfaces
{
    public interface ITelemetryService
    {
        void RecordProductBought(long productId, int count = 1);

        Task<Result<int>> GetProductCountMetric(long productId);

        Task<Result<decimal>> GetUserTransaction(long userId);

        Task<Result<double>> GetProductGeneralRating(long productId);

        Task<Result<List<KeyValuePair<int, int>>>> GetProductExtentionRating(long productId);
    }
}
