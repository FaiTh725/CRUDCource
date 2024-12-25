using Authorize.Domain.Entities;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Domain.Repositories
{
    public interface IRolesRepository
    {
        Task<Result<Roles>> GetRole(string nameRole);

    }
}
