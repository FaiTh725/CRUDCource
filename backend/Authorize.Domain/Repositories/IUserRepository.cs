using Authorize.Domain.Entities;
using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Domain.Repositories
{
    public interface IUserRepository
    {
        public Task<Result<User>> AddUser(User user);

        public Task<Result<User>> GetUser(string email);
    }
}
