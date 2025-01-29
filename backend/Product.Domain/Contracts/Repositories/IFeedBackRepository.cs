using CSharpFunctionalExtensions;
using Product.Domain.Contracts.Models.Product;
using Product.Domain.Entities;

namespace Product.Domain.Contracts.Repositories
{
    public interface IFeedBackRepository
    {
        Task<Result<FeedBack>> AddFeedBack(FeedBack feedBack);

        IQueryable<FeedBack> GetFeedBacksProduct(long productId, int? rating = null);

        IQueryable<FeedBack> GetUserFeedBacks(string emailUser);

        IQueryable<ProductRating> GetProductsRating();
    }
}
