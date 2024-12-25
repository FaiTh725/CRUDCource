using Application.Contracts.Response;
using Authorize.Contracts.User;
using Authorize.Dal.Implementation;
using Authorize.Domain.Entities;
using Authorize.Domain.Repositories;
using Authorize.Helpers.Settings;
using Authorize.Services.Interfaces;
using CSharpFunctionalExtensions;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Notification.Domain.Contacrs.Email;
using Product.Domain.Contracts.Models.Account;
using Product.Domain.Contracts.Models.Request;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Authorize.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository userRepository;
        private readonly IRolesRepository rolesRepository;
        private readonly IJwtService jwtService;
        private readonly ICacheService cacheService;
        private readonly IPublishEndpoint publishEndpoint;
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;

        public AuthenticationService(
            IUserRepository userRepository,
            IJwtService jwtService,
            IRolesRepository rolesRepository,
            ICacheService cacheService,
            IPublishEndpoint publishEndpoint,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.jwtService = jwtService;
            this.rolesRepository = rolesRepository;
            this.cacheService = cacheService;
            this.publishEndpoint = publishEndpoint;
            this.httpClient = httpClient;
            this.configuration = configuration;
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

        public async Task<DataResponse<string>> Login(UserLogin request)
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

            var tokenToAnotherApi = jwtService.GenerateToken(userResult.Value);
            var apiList = configuration.GetSection("APIList").Get<APIList>()
                ?? throw new NullReferenceException("Configuration api list is empty");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenToAnotherApi);
            var responseGetAccountInfo = await httpClient.GetAsync(
                    $@"{apiList.ProductAPI}/Account/AccountInfo?email={request.Email}");

            if(!responseGetAccountInfo.IsSuccessStatusCode)
            {
                return new DataResponse<string>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Error with communication to endpoint",
                    Data = string.Empty
                };
            }

            var responseJson = await responseGetAccountInfo.Content.ReadAsStringAsync();

            var jsonDesiarializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var responseData = JsonSerializer.Deserialize<DataResponse<AccountResponseDetail>>(
                responseJson, jsonDesiarializeOptions);

            if(responseData is null || responseData.StatusCode != StatusCode.Ok)
            {
                return new DataResponse<string>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Error with communication to endpoint",
                    Data = string.Empty
                };
            }

            var token = jwtService.GenerateToken(responseData.Data.Name, userResult.Value);

            return new DataResponse<string>
            {
                Data = token,
                Description = "Successfull login",
                StatusCode = StatusCode.Ok
            };
        }

        public async Task<DataResponse<string>> Register(UserRegister request)
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

            await publishEndpoint.Publish(new CreateAccount
            {
                Name = request.UserName, 
                Email = request.Email,
            });

            var token = jwtService.GenerateToken(request.UserName, registeredResult.Value);

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
