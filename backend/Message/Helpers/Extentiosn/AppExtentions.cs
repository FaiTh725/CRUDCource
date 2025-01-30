using Application.Contracts.SharedModels.Exceptions;
using Message.Helpers.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Redis.OM.Searching.Query;
using StackExchange.Redis;
using System.Net.Security;
using System.Text;

namespace Message.Helpers.Extentiosn
{
    public static class AppExtentions
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configurations)
        {
            services.AddSingleton<IConnectionMultiplexer>(options =>
                {
                    var redisOptions = ConfigurationOptions.Parse(
                        configurations.GetConnectionString("RedisConnection") ??
                        throw new AppConfigurationException("Redis Connection"));

                    redisOptions.AbortOnConnectFail = false;

                    return ConnectionMultiplexer.Connect(redisOptions);
                }
            );
        }

        public static void AddJwtService(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSetting = configuration.GetSection("JwtSetting").Get<JwtConf>()
                ?? throw new AppConfigurationException("JwtSetting");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = jwtSetting.Audience,
                        ValidIssuer = jwtSetting.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            var token = ctx.Request.Cookies["token"];
                            if(!string.IsNullOrEmpty(token))
                            {
                                ctx.Token = token;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
        }

        public static void AddCustomHttpClietns(this IServiceCollection services)
        {
            services.AddHttpClient("HttpClientTrustCert")
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = 
                    (sender, cert, chain, sllPolicy) => { return true; }
                });
        }
    }
}
