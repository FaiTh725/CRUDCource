
using Application.Contracts.SharedModels.Exceptions;
using Authorize.Contracts.User;
using Authorize.Dal;
using Authorize.Helpers.Jwt;
using Authorize.Helpers.Settings;
using Authorize.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net.Security;
using System.Text;

namespace Authorize.Helpers.Extentions
{
    public static class AppExtention
    {

        public static void AddJwtService(this IServiceCollection service, IConfiguration configuration)
        {
            var jwtSetting = configuration.GetSection("JwtSetting").Get<JwtSetting>();

            if(jwtSetting is null)
            {
                throw new AppConfigurationException("JwtSetting");
            }

            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = jwtSetting!.Issuer,
                        ValidAudience = jwtSetting!.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            var token = ctx.Request.Cookies["token"];
                            if (!string.IsNullOrEmpty(token))
                            {
                                ctx.Token = token;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            service.AddAuthorization();
        }

        public static void AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            var messageBrokerSetting = configuration.GetSection("RabbitMqSetting").Get<MessageBrokerSetting>();

            if(messageBrokerSetting is null)
            {
                throw new AppConfigurationException("MessageBroker setting");
            }

            services.AddMassTransit(conf =>
            {
                conf.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri($"amqp://{messageBrokerSetting!.Host}"), x =>
                    {
                        x.Username(messageBrokerSetting.Login);
                        x.Password(messageBrokerSetting.Password);

                    });

                    configurator.ConfigureEndpoints(context);
                });
            });
        }

        public static void AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("RedisConnection") ??
                throw new AppConfigurationException("Connection string to redis");

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "Users";
            });
        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<UserLogin>, UserValidator>();
            services.AddScoped<IValidator<UserRegister>, UserValidatorRegister>();
        }

        public static void AddCustomHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient("ProductHttp")
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = 
                    (sender, cert, chain, sllPolicy) => { return true; }
                });
        }

        public static void AddCorses(this IServiceCollection service, IConfiguration configuration)
        {
            var frontendBaseUrl = configuration.GetValue<string>("APIList:Frontend")
                ?? throw new AppConfigurationException("APIList:Frontend");

            service.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy.WithOrigins(frontendBaseUrl)
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }
    }
}
