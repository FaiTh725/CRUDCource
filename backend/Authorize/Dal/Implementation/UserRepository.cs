using Authorize.Domain.Entities;
using Authorize.Domain.Repositories;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace Authorize.Dal.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result<User>> AddUser(User user)
        {
            try
            {

                var newUser = await dbContext.Users.AddAsync(user);

                await dbContext.SaveChangesAsync();

                return Result.Success<User>(newUser.Entity);
            }
            catch
            {
                return Result.Failure<User>("Error insert new user");
            }
        }

        public async Task<Result<User>> GetUser(string email)
        {
            try
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);

                return user == null ? Result.Failure<User>($"User with {email} not found") : Result.Success<User>(user);
            }
            catch
            {
                return Result.Failure<User>("Error get user");
            }
        }
    }
}
