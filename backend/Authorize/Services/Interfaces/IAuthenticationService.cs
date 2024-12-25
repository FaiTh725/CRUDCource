using Application.Contracts.Response;
using Authorize.Contracts.User;

namespace Authorize.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<DataResponse<string>> Login(UserLogin request);

        public Task<DataResponse<string>> Register(UserRegister request);

        public Task<BaseResponse> VerifyEmail(string email);

        public Task<BaseResponse> ConfirmEmail(string email, int key);
    }
}
