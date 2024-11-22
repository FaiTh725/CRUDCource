using Application.Contracts.Response;
using Authorize.Contracts.User;

namespace Authorize.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<DataResponse<string>> Login(CreateUser request);

        public Task<DataResponse<string>> Register(CreateUser request);

        public Task<BaseResponse> VerifyEmail(string email);

        public Task<BaseResponse> ConfirmEmail(string email, int key);
    }
}
