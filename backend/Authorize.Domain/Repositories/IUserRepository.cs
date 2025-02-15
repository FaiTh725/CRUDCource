﻿using Authorize.Domain.Entities;
using CSharpFunctionalExtensions;

namespace Authorize.Domain.Repositories
{
    public interface IUserRepository
    {
        public Task<Result<User>> AddUser(User user);

        public Task<Result<User>> GetUser(string email);

        public Task<Result> UpdateUser(User userToUpdate, Roles newRole, string password);
    }
}
