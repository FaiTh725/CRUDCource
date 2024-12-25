using CSharpFunctionalExtensions;
using Product.Domain.Models;
using Product.Domain.Entities;



namespace Product.Domain.Contracts.Repositories
{
    public interface IAccountRepository
    {
        Task<Result<Account>> AddAccount(Account account);

        Task<Result<Account>> GetAccountByEmail(string email);

        Task<Result<Account>> GetAccountWithOrderHistory(string email);

        Task<Result<Account>> GetAccountWithCart(string email);

        Task<Result> AddProductToCart(Account account, CartItem product);
    }
}
