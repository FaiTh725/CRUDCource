using Authorize.Domain.Entities;
using CSharpFunctionalExtensions;

namespace Authorize.Domain.Repositories
{
    public interface IRolesRepository
    {
        Task<Result<Roles>> GetRole(string nameRole);

    }
}
