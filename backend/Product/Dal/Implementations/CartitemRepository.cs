using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Product.Domain.Contracts.Repositories;

namespace Product.Dal.Implementations
{
    public class CartitemRepository : ICartItemRepository
    {
        private readonly AppDbContext context;

        public CartitemRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Result> RemoveCartByProductId(long productId)
        {
            try
            {
                await context.CartItems
                    .Include(x => x.Product)
                    .Where(x => x.Product.Id == productId)
                    .ExecuteDeleteAsync();

                return Result.Success();
            }
            catch
            {
                return Result.Failure("Nothing to delete"); ;
            }
        }
    }
}
