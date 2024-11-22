using Application.Contracts.Response;
using Authorize.Contracts.User;
using Authorize.Dal.Implementation;
using Authorize.Domain.Entities;
using Authorize.Domain.Repositories;
using Authorize.Services.Interfaces;
using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Notification.Domain.Contacrs.Email;

namespace Authorize.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository userRepository;
        private readonly IRolesRepository rolesRepository;
        private readonly IJwtService jwtService;
        private readonly ICacheService cacheService;
        private readonly IPublishEndpoint publishEndpoint;

        public AuthenticationService(
            IUserRepository userRepository,
            IJwtService jwtService,
            IRolesRepository rolesRepository,
            ICacheService cacheService,
            IPublishEndpoint publishEndpoint)
        {
            this.userRepository = userRepository;
            this.jwtService = jwtService;
            this.rolesRepository = rolesRepository;
            this.cacheService = cacheService;
            this.publishEndpoint = publishEndpoint;
        }

        public async Task<BaseResponse> ConfirmEmail(string email, int key)
        {
            var dataResult = await cacheService.GetData<int>(email);

            if(dataResult.IsFailure || dataResult.Value != key)
            {
                return new BaseResponse
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "Invalid Email"
                };
            }

            var resultRemoveKey = await cacheService.DeleteData(dataResult.Value.ToString());

            if(!resultRemoveKey)
            {
                return new BaseResponse
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Unknown server error"
                };
            }

            return new BaseResponse
            {
                StatusCode = StatusCode.Ok,
                Description = "Success confirm email"
            };
        }

        public async Task<DataResponse<string>> Login(CreateUser request)
        {
            var userResult = await userRepository.GetUser(request.Email);

            if(!userResult.IsSuccess)
            {
                return new DataResponse<string>
                {
                    Data = string.Empty,
                    Description = $"User with email {request.Email} not exist",
                    StatusCode = StatusCode.NotFound
                };
            }

            if(request.Password != userResult.Value.Password)
            {
                return new DataResponse<string>
                {
                    Data = string.Empty,
                    Description = $"Invalid email or password",
                    StatusCode = StatusCode.NotFound
                };
            }

            var token = jwtService.GenerateToken(userResult.Value);

            return new DataResponse<string>
            {
                Data = token,
                Description = "Successfull login",
                StatusCode = StatusCode.Ok
            };
        }

        public async Task<DataResponse<string>> Register(CreateUser request)
        {
            var userResult = await userRepository.GetUser(request.Email);

            if(userResult.IsSuccess)
            {
                return new DataResponse<string>
                {
                    Data = string.Empty,
                    Description = "Current email alredy registered",
                    StatusCode = StatusCode.BadRequest
                };
            }

            var roleUserResult = await rolesRepository.GetRole("User");

            if(roleUserResult.IsFailure)
            {
                return new DataResponse<string>
                {
                    Data = string.Empty,
                    Description = "Internal server error",
                    StatusCode = StatusCode.InternalServerError
                };
            }

            var newUserResult = User.Initialize(0, request.Email, request.Password, roleUserResult.Value);

            if(newUserResult.IsFailure)
            {
                return new DataResponse<string>
                {
                    Data = string.Empty,
                    Description = newUserResult.Error,
                    StatusCode = StatusCode.BadRequest
                };
            }

            var registeredResult = await userRepository.AddUser(newUserResult.Value);

            if(!registeredResult.IsSuccess)
            {
                return new DataResponse<string>
                {
                    Data = string.Empty,
                    Description = "Internal server",
                    StatusCode = StatusCode.InternalServerError
                };
            }

            var token = jwtService.GenerateToken(registeredResult.Value);

            return new DataResponse<string>
            {
                Data = token,
                Description = "Successfull registered user",
                StatusCode= StatusCode.Ok
            };
        }

        public async Task<BaseResponse> VerifyEmail(string email)
        {
            var userResult = await userRepository.GetUser(email);

            if (userResult.IsSuccess)
            {
                return new BaseResponse
                {
                    Description = "Email alredy registered",
                    StatusCode = StatusCode.BadRequest
                };
            }

            
            var random = new Random();
            var key = random.Next(1000, 10000);

            await cacheService.SetData(email, key);

            await publishEndpoint.Publish(new SendEmail
            {
                Subject = "Confirm Login",
                Body = $"The key to confirm - {key}",
                Recipients= { email }
            });


            return new BaseResponse
            {
                Description = "Send verify email",
                StatusCode = StatusCode.Ok
            };
        }
    }
}
