using Authorize.Domain.Entities;
using Authorize.Domain.Repositories;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace Authorize.Dal.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public UserRepository(AppDbContext dbContext)
        {
            this.context = dbContext;
        }

        public async Task<Result<User>> AddUser(User user)
        {
            try
            {

                var newUser = await context.Users.AddAsync(user);

                await context.SaveChangesAsync();

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
                var user = await context.Users
                    .Include(x => x.Role)
                    .FirstOrDefaultAsync(x => x.Email == email);

                return user == null ? Result.Failure<User>($"User with {email} not found") : Result.Success<User>(user);
            }
            catch
            {
                return Result.Failure<User>("Error get user");
            }
        }

        public async Task<Result> UpdateUser(User userToUpdate, Roles newRole, string password)
        {
            try
            {
                userToUpdate.Role = newRole;
                userToUpdate.Password = password;

                await context.SaveChangesAsync();

                return Result.Success();
            }
            catch
            {
                return Result.Failure("Error update user");
            }
        }
    }
}
