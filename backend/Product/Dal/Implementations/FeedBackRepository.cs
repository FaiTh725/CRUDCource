using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Product.Domain.Contracts.Models.Product;
using Product.Domain.Contracts.Repositories;
using Product.Domain.Entities;

namespace Product.Dal.Implementations
{
    public class FeedBackRepository : IFeedBackRepository
    {
        private readonly AppDbContext context;

        public FeedBackRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Result<FeedBack>> AddFeedBack(FeedBack feedBack)
        {
            var entity = await context.FeedBacks.AddAsync(feedBack);
        
            var countChanges = await context.SaveChangesAsync();

            return countChanges > 0 ?
                Result.Success(entity.Entity) :
                Result.Failure<FeedBack>("Error Add Feed Back");

        }

        public IQueryable<FeedBack> GetFeedBacksProduct(long productId, int? rating = null)
        {

            var feedBacks = context.FeedBacks
                .Include(x => x.Owner)
                .Include(x => x.Product)
                .Where(x => x.Product.Id == productId);

            if (rating is not null)
            {
                feedBacks = feedBacks.Where(x => x.Rate == rating);
            }

            return
                feedBacks
                .OrderByDescending(x => x.SendTime);
        }

        public IQueryable<ProductRating> GetProductsRating()
        {
            return context.FeedBacks
                .Include(x => x.Product)
                .GroupBy(x => x.Product.Id)
                .Select(x => new ProductRating
                {
                    ProductId = x.Key,
                    GeneralRating = (double)x.Sum(y => y.Rate) / x.Count(),
                    PartRatingCount = x
                        .GroupBy(x => x.Rate)
                        .Select(y => new KeyValuePair<int, int>(
                            y.Key,
                            y.Count()))
                        .ToList()
                });
        }

        public IQueryable<FeedBack> GetUserFeedBacks(string emailUser)
        {
            return context.FeedBacks
                .Include(x => x.Owner)
                .Where(x => x.Owner.Email == emailUser);
        }
    }
}
