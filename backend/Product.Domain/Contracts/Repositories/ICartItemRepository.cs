using CSharpFunctionalExtensions;
using Product.Domain.Entities;

namespace Product.Domain.Contracts.Repositories
{
    public interface ICartItemRepository
    {
        Task<Result> RemoveCart(CartItem cartItem);

        Task<Result> RemoveCarts(List<CartItem> cartItems);
    }
}
