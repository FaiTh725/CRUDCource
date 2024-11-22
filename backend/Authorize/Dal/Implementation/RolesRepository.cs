using Authorize.Domain.Entities;
using Authorize.Domain.Repositories;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace Authorize.Dal.Implementation
{
    public class RolesRepository : IRolesRepository
    {
        private readonly AppDbContext dbContext;

        public RolesRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result<Roles>> GetRole(string nameRole)
        {
            try
            {
                var role = await dbContext.Roles.FirstOrDefaultAsync(x => x.Role == nameRole);

                if(role == null)
                {
                    return Result.Failure<Roles>($"Role with name {nameRole} not found");
                }

                return Result.Success<Roles>(role);
            }
            catch
            {
                return Result.Failure<Roles>("Get role error");
            }
        }
    }
}
