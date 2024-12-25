using Application.Contracts.Response;
using Product.Domain.Contracts.Models.Request;

namespace Authorize.Services.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse> UpdateRoleUser(ChangeRoleAccount accountRequest);
    }
}
