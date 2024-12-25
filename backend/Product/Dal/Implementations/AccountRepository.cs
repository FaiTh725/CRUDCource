using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Product.Domain.Contracts.Repositories;
using Product.Domain.Entities;
using Product.Domain.Models;
using ProductEntity = Product.Domain.Models.Product;

namespace Product.Dal.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDbContext context;

        public AccountRepository(AppDbContext dbContext)
        {
            this.context = dbContext;
        }

        public async Task<Result<Account>> AddAccount(Account account)
        {
            var newAccountEntity = await context.Accounts.AddAsync(account);

            var countChanges = await context.SaveChangesAsync();

            return countChanges > 0 ? 
                Result.Success(newAccountEntity.Entity) : 
                Result.Failure<Account>("Error with add new account");
        }

        public async Task<Result> AddProductToCart(Account account, CartItem product)
        {
            account.ShopingCart.Add(product);
            
            int countChanges = await context.SaveChangesAsync();

            return countChanges > 0 ?
                Result.Success() :
                Result.Failure("Something called error when was updating");
        }

        public async Task<Result<Account>> GetAccountByEmail(string email)
        {
            var account = await context.Accounts
                .FirstOrDefaultAsync(x => x.Email == email);

            return account == null ?
                Result.Failure<Account>("Account with this email not exist") :
                Result.Success(account);
        }

        public async Task<Result<Account>> GetAccountWithCart(string email)
        {
            var account = await context.Accounts
                .Include(x => x.ShopingCart)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(x => x.Email == email);

            return account is null ? 
                Result.Failure<Account>("Account not found") :
                Result.Success(account);
        }

        public async Task<Result<Account>> GetAccountWithOrderHistory(string email)
        {
            var account = await context.Accounts
                .Include(x => x.ShopingHistory)
                .FirstOrDefaultAsync(x => x.Email == email);

            return account == null ?
                Result.Failure<Account>("Account with this email not exist") : 
                Result.Success(account);
        }
    }
}
