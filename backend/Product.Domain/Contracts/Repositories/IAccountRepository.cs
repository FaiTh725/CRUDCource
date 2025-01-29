using CSharpFunctionalExtensions;
using Product.Domain.Models;
using Product.Domain.Entities;
using Product.Domain.Contracts.Models.Account;


namespace Product.Domain.Contracts.Repositories
{
    public interface IAccountRepository
    {
        Task<Result<Account>> AddAccount(Account account);

        Task<Result<Account>> GetAccountByEmail(string email);

        Task<Result<Account>> GetAccountWithOrderHistory(string email);

        Task<Result<Account>> GetAccountWithCart(string email);

        Task<Result> AddProductsToCart(Account account, List<CartItem> products);

        Task<Result> AddProductToOrderHistory(Account account, List<CartItem> products);

        IQueryable<AccountTransactions> GetAccountsWithTotalTransactions();
    }
}
