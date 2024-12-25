using Application.Contracts.Response;
using Authorize.Domain.Repositories;
using Authorize.Services.Interfaces;
using Product.Domain.Contracts.Models.Request;

namespace Authorize.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IRolesRepository rolesRepository;

        public UserService(
            IUserRepository userRepository,
            IRolesRepository rolesRepository)
        {
            this.userRepository = userRepository;
            this.rolesRepository = rolesRepository;
        }

        public async Task<BaseResponse> UpdateRoleUser(ChangeRoleAccount accountRequest)
        {
            try
            {
                var user = await userRepository.GetUser(accountRequest.Email);

                if(user.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "User with such email is not exist"
                    };
                }

                var roleToChange = await rolesRepository.GetRole(accountRequest.NewRole);

                if(roleToChange.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Such role is not exist"
                    };
                }

                var resultUpdate = await userRepository.UpdateUser(
                    user.Value, 
                    roleToChange.Value, 
                    user.Value.Password);
            
                if(resultUpdate.IsFailure)
                {
                    return new BaseResponse
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = "Error update user"
                    };
                }

                return new BaseResponse
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Success update user"
                };
            }
            catch
            {
                return new BaseResponse
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Internal Server Error"
                };
            }
        }
    }
}
