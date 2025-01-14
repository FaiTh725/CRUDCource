using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Product.Domain.Contracts.Repositories;
using Product.Domain.Entities;

namespace Product.Dal.Implementations
{
    public class CartitemRepository : ICartItemRepository
    {
        private readonly AppDbContext context;

        public CartitemRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Result> RemoveCarts(List<CartItem> cartItems)
        {
            try
            {
                await context.CartItems
                    .Where(x => cartItems.Contains(x))
                    .ExecuteDeleteAsync();

                await context.SaveChangesAsync();

                return Result.Success();
            }
            catch
            {
                return Result.Failure("Error Items Delete");
            }
            
        }

        public async Task<Result> RemoveCart(CartItem cartItem)
        {
            try
            {
                await context.CartItems
                    .Where(x => x == cartItem)
                    .ExecuteDeleteAsync();

                return Result.Success();
            }
            catch
            {
                return Result.Failure("Error Item Delete"); ;
            }
        }
    }
}
