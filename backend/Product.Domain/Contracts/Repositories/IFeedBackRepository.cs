using CSharpFunctionalExtensions;
using Product.Domain.Contracts.Models.Product;
using Product.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Repositories
{
    public interface IFeedBackRepository
    {
        Task<Result<FeedBack>> AddFeedBack(FeedBack feedBack);

        IQueryable<FeedBack> GetFeedBacksProduct(long productId);

        IQueryable<FeedBack> GetUserFeedBacks(string emailUser);

        IQueryable<ProductRating> GetProductsRating();
    }
}
