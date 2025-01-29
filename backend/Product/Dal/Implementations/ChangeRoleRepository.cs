using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Product.Domain.Contracts.Repositories;
using Product.Domain.Entities;

namespace Product.Dal.Implementations
{
    public class ChangeRoleRepository : IChangeRoleRepository
    {
        private readonly AppDbContext context;

        public ChangeRoleRepository(AppDbContext context)
        {
            this.context = context;
        }
        
        public async Task<Result> ClearCompleteRequest(int countHours)
        {
            try
            {
                await context.ChangeRoleRequests
                    .Where(x => x.IsReviewed && x.CreatedDate.AddDays(countHours) < DateTime.Now)
                    .ExecuteDeleteAsync();

                return Result.Success();
            }
            catch
            {
                return Result.Failure("Error while deleting data");
            }
        }

        public async Task<Result> CompleteRequest(ChangeAccountRoleRequest request, bool commitStatus)
        {
            if(request.IsReviewed)
            {
                return Result.Failure("Change request is already close");
            }

            request.CommitRequest(commitStatus);

            var countChanges = await context.SaveChangesAsync();

            return countChanges > 0 ?
                Result.Success() :
                Result.Failure("Error update status request");
        }

        public async Task<Result<ChangeAccountRoleRequest>> CreateRequest(ChangeAccountRoleRequest request)
        {
            var newRequest = await context.ChangeRoleRequests.AddAsync(request);

            var countChanges = await context.SaveChangesAsync();

            return countChanges > 0 ?
                Result.Success(newRequest.Entity) :
                Result.Failure<ChangeAccountRoleRequest>("Error create new request");
        }

        public async Task<Result<ChangeAccountRoleRequest>> GetRequest(long idRequest)
        {
            var request = await context.ChangeRoleRequests.FirstOrDefaultAsync(x => x.Id == idRequest);

            return request == null ?
                Result.Failure<ChangeAccountRoleRequest>("Dont found request") :
                Result.Success(request);
        }

        public async Task<Result<ChangeAccountRoleRequest>> GetUnCommitedRequestUser(string email)
        {
            var request = await context.ChangeRoleRequests
                .FirstOrDefaultAsync(x => x.Email == email && 
                    x.IsReviewed == false);

            return request == null ?
                Result.Failure<ChangeAccountRoleRequest>("Request already sended") :
                Result.Success(request);
        }
    }
}
