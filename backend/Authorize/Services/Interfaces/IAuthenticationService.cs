using Application.Contracts.Response;
using Authorize.Contracts.User;
using Authorize.Domain.Modals.Auth;

namespace Authorize.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<DataResponse<TokenData>> Login(UserLogin request);

        public Task<DataResponse<TokenData>> Register(UserRegister request);

        public Task<BaseResponse> VerifyEmail(string email);

        public Task<BaseResponse> ConfirmEmail(string email, int key);
    }
}
