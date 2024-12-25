using CSharpFunctionalExtensions;
using Product.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Domain.Contracts.Repositories
{
    public  interface IChangeRoleRepository
    {
        Task<Result<ChangeAccountRoleRequest>> CreateRequest(ChangeAccountRoleRequest request);
    
        Task<Result> CompleteRequest(ChangeAccountRoleRequest request, bool commitStatus);

        Task<Result<ChangeAccountRoleRequest>> GetRequest(long idRequest);

        Task<Result> ClearCompleteRequest(int countHours);

        Task<Result<ChangeAccountRoleRequest>> GetUnCommitedRequestUser(string email);
    }
}
