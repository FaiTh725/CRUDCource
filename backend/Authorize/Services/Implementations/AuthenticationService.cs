using Application.Contracts.Response;
using Application.Contracts.SharedModels.Exceptions;
using Authorize.Contracts.User;
using Authorize.Domain.Entities;
using Authorize.Domain.Modals.Auth;
using Authorize.Domain.Repositories;
using Authorize.Services.Interfaces;
using MassTransit;
using Notification.Domain.Contacrs.Email;
using Product.Domain.Contracts.Models.Account;
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
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        private readonly ICookieService cookieService;

        public AuthenticationService(
            IUserRepository userRepository,
            IJwtService jwtService,
            IRolesRepository rolesRepository,
            ICacheService cacheService,
            IPublishEndpoint publishEndpoint,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ICookieService cookieService)
        {
            this.userRepository = userRepository;
            this.jwtService = jwtService;
            this.rolesRepository = rolesRepository;
            this.cacheService = cacheService;
            this.publishEndpoint = publishEndpoint;
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
            this.cookieService = cookieService;
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

            var resultRemoveKey = await cacheService.DeleteData(email);

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

        public async Task<DataResponse<TokenData>> Login(UserLogin request)
        {
            var userResult = await userRepository.GetUser(request.Email);

            if (!userResult.IsSuccess)
            {
                return new DataResponse<TokenData>
                {
                    Data = new(),
                    Description = $"User with email {request.Email} not exist",
                    StatusCode = StatusCode.NotFound
                };
            }

            if (request.Password != userResult.Value.Password)
            {
                return new DataResponse<TokenData>
                {
                    Data = new(),
                    Description = $"Invalid email or password",
                    StatusCode = StatusCode.NotFound
                };
            }

            var tokenToAnotherApi = jwtService.GenerateToken(userResult.Value);

            var apiList = configuration.GetValue<string>("APIList:ProductAPI")
                ?? throw new AppConfigurationException("ApiList");

            var httpClient = httpClientFactory.CreateClient("ProductHttp");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenToAnotherApi);
            var responseGetAccountInfo = await httpClient.GetAsync(
                    $@"{apiList}/Account/AccountInfo?email={request.Email}");

            if (!responseGetAccountInfo.IsSuccessStatusCode)
            {
                return new DataResponse<TokenData>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Error with communication to endpoint",
                    Data = new()
                };
            }

            var responseJson = await responseGetAccountInfo.Content.ReadAsStringAsync();

            var jsonDesiarializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var responseData = JsonSerializer.Deserialize<DataResponse<AccountResponseDetail>>(
                responseJson, jsonDesiarializeOptions);

            if (responseData is null || responseData.StatusCode != StatusCode.Ok)
            {
                return new DataResponse<TokenData>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Error with communication to endpoint",
                    Data = new()
                };
            }

            var token = jwtService.GenerateToken(responseData.Data.Name, userResult.Value);

            var decodeToken = jwtService.DecodeToken(token);

            if (decodeToken.IsFailure)
            {
                return new DataResponse<TokenData>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Error generate token on server",
                    Data = new()
                };
            }
            cookieService.SetCookie("token", token);

            return new DataResponse<TokenData>
            {
                Data = decodeToken.Value,
                Description = "Successfull login",
                StatusCode = StatusCode.Ok
            };
        }

        public async Task<DataResponse<TokenData>> Register(UserRegister request)
        {
            var userResult = await userRepository.GetUser(request.Email);

            if(userResult.IsSuccess)
            {
                return new DataResponse<TokenData>
                {
                    Data = new(),
                    Description = "Current email alredy registered",
                    StatusCode = StatusCode.BadRequest
                };
            }

            var roleUserResult = await rolesRepository.GetRole("User");

            if(roleUserResult.IsFailure)
            {
                return new DataResponse<TokenData>
                {
                    Data = new(),
                    Description = "Internal server error",
                    StatusCode = StatusCode.InternalServerError
                };
            }

            var newUserResult = User.Initialize(0, request.Email, request.Password, roleUserResult.Value);

            if(newUserResult.IsFailure)
            {
                return new DataResponse<TokenData>
                {
                    Data = new(),
                    Description = newUserResult.Error,
                    StatusCode = StatusCode.BadRequest
                };
            }

            var registeredResult = await userRepository.AddUser(newUserResult.Value);

            if(!registeredResult.IsSuccess)
            {
                return new DataResponse<TokenData>
                {
                    Data = new(),
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

            var decodeToken = jwtService.DecodeToken(token);

            if (decodeToken.IsFailure)
            {
                return new DataResponse<TokenData>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Error generate token on server",
                    Data = new()
                };
            }

            cookieService.SetCookie("token", token);

            return new DataResponse<TokenData>
            {
                Data = decodeToken.Value,
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
